using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WebServiceEnrollment.Model;
using System.Data;

namespace WebServiceEnrollment.DB
{
    public class Conexion
    {
        private static Conexion instancia;
        private SqlConnection connection;
        private IConfiguration Configuration;

        public Conexion(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            //Obtiene la conexión a la base de datos del archivo de configuración
            connection = new SqlConnection(Configuration.GetConnectionString("defaultConnection"));
        }

        //como buena practica los metodos deben declararse con inicial mayúscula
        public static Conexion GetInstancia(IConfiguration Configuration)
        {
            if (instancia == null)
            {
                instancia = new Conexion(Configuration);
            }
            return instancia;
        }

        public AspiranteResponse ExecuteQuery(AspiranteRequest request)
        {
            AspiranteResponse response = null;
            SqlCommand cmd = new SqlCommand("sp_CandidateRecordCreate", connection);
            //SqlCommand cmd = new SqlCommand(Configuration.GetValue<string>("Profiles:spCandidateRecordCreate"), connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@apellidos", request.Apellido));
            cmd.Parameters.Add(new SqlParameter("@nombres", request.Nombre));
            cmd.Parameters.Add(new SqlParameter("@direccion", request.Direccion));
            cmd.Parameters.Add(new SqlParameter("@telefono", request.Telefono));
            cmd.Parameters.Add(new SqlParameter("@email", request.Email));
            cmd.Parameters.Add(new SqlParameter("@carreraId", request.CarreraId));
            cmd.Parameters.Add(new SqlParameter("@examenId", request.ExamenId));
            cmd.Parameters.Add(new SqlParameter("@jornadaId", request.JornadaId));

            SqlDataReader reader = null;

            try
            {
                this.connection.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    response = new AspiranteResponse()
                    {
                        Message = reader.GetValue(0).ToString(),
                        NoExpediente = reader.GetValue(1).ToString()
                    };
                    if (reader.GetValue(0).ToString().Equals("TRANSACTION SUCCESS"))
                    {
                        response.StatusCode = 201;
                    }
                    else if (reader.GetValue(0).ToString().Equals("TRANSACTION ERROR"))
                    {
                        response.StatusCode = 503;
                    }
                    else
                    {
                        response.StatusCode = 500;
                    }
                }
                reader.Close();
                this.connection.Close();
            }
            catch (Exception e)
            {
                response = new AspiranteResponse()
                {
                    StatusCode = 500,
                    Message = "Error al momenot de ejecutar el proceso de creación de un aspirante en la base de datos",
                    NoExpediente = "0"
                };
            }
            finally
            {
                this.connection.Close();
            }

            return response;
        }
        public EnrollmentResponse ExecuteQuery(EnrollmentRequest request)
        {
            EnrollmentResponse response = null;

            // SqlCommand cmd = new SqlCommand("sp_EnrollmentProcess", connection);
            SqlCommand cmd = new SqlCommand(Configuration.GetValue<string>("Profiles:spEnrollmentProcess"), connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@noExpediente", request.NoExpediente));
            cmd.Parameters.Add(new SqlParameter("@ciclo", request.Ciclo));
            cmd.Parameters.Add(new SqlParameter("@mesInicioPago", request.MesInicioPago));
            cmd.Parameters.Add(new SqlParameter("@carreraId", request.CarreraId));
            cmd.Parameters.Add(new SqlParameter("@inscripcionCargoId", request.InscripcionCargoId));
            cmd.Parameters.Add(new SqlParameter("@carneCargoId", request.CarneCargoId));
            cmd.Parameters.Add(new SqlParameter("@cargoMensualId", request.CargoMensualId));
            cmd.Parameters.Add(new SqlParameter("@diapago", request.DiaPago));

            SqlDataReader reader = null;

            try
            {
                this.connection.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    response = new EnrollmentResponse()
                    {
                        Message = reader.GetValue(0).ToString(),
                        Carne = reader.GetValue(1).ToString()
                    };
                    if (reader.GetValue(0).ToString().Equals(Configuration.GetValue<string>("Profiles:messages:tSuccess")))
                    {
                        response.StatusCode = Configuration.GetValue<int>("Profiles:errorService:created");//201;
                    }
                    else if (reader.GetValue(0).ToString().Equals(Configuration.GetValue<string>("Profiles:messages:tError")))
                    {
                        response.StatusCode = Configuration.GetValue<int>("Profiles:errorService:nonAuthoritativeInformation");//203;
                    }
                    else
                    {
                        response.StatusCode = Configuration.GetValue<int>("Profiles:errorService:internalServerError");//500;
                    }
                }
                reader.Close();
                this.connection.Close();
            }
            catch (Exception e)
            {
                response.StatusCode = Configuration.GetValue<int>("Profiles:errorService:serviceUnavailable");//503;
                response.Message = Configuration.GetValue<string>("Profiles:errorAccount");
                // response.Message = "Error al momento de ejecutar el proceso de incripción en la base de datos";
            }
            finally
            {
                this.connection.Close();
            }

            return response;
        }

        public AccountStatementResponse GetAccountStatement(AccountStatementRequest request)
        {
            AccountStatementResponse response = new AccountStatementResponse();

            //SqlCommand cmd = new SqlCommand("sp_GetAccountStatement", connection);
            SqlCommand cmd = new SqlCommand(Configuration.GetValue<string>("Profiles:spGetAccountStatement"), connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@carne", request.Carne));

            SqlDataReader reader = null;

            try
            {
                List<AccountStatement> lstAccount = new List<AccountStatement>();
                this.connection.Open();
                reader = cmd.ExecuteReader();

                if (reader.HasRows == false)
                {
                    response.StatusCode = Configuration.GetValue<int>("Profiles:errorService:noContent");//204;
                }

                while (reader.Read())
                {
                    AccountStatement account = new AccountStatement()
                    {
                        Carne = reader.GetValue(0).ToString(),
                        Nombre = reader.GetValue(1).ToString(),
                        Descripcion = reader.GetValue(2).ToString(),
                        Monto = decimal.Parse(reader.GetValue(3).ToString()),
                        Mora = decimal.Parse(reader.GetValue(4).ToString()),
                        Descuento = decimal.Parse(reader.GetValue(5).ToString()),
                    };
                    lstAccount.Add(account);
                    response.AccountStatements = lstAccount;
                    if (reader.HasRows == true)
                    {
                        response.StatusCode = Configuration.GetValue<int>("Profiles:errorService:ok");//200;
                        response.AccountStatements = lstAccount;
                    }
                }
                reader.Close();
                this.connection.Close();
            }
            catch (Exception e)
            {
                response.StatusCode = Configuration.GetValue<int>("Profiles:errorService:serviceUnavailable");//503;
                response.Message = Configuration.GetValue<string>("Profiles:errorAccount");
                //response.Message = "Error al momento de consultar el estado de cuenta a la base de datos";
            }
            finally
            {
                this.connection.Close();
            }

            return response;
        }
        
    }
}