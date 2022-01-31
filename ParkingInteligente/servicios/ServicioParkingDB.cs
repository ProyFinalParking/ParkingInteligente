using Microsoft.Data.Sqlite;
using ParkingInteligente.modelo;
using System;

namespace ParkingInteligente.servicios
{
    class ServicioParkingDB
    {
        // Propiedad de Configuración de aplicaciones con el nombre de la BBDD
        readonly string nombreBD = Properties.Settings.Default.NombreBD;

        public ServicioParkingDB()
        {
            // Crea las tablas si no existen al instanciar el objeto
            CreateTablesIfNotExists();
        }

        /**************************************************************************************************************************** 
         * Ver la forma de controlar excepciones de SQLite, para devolver TRUE en caso de exito y FALSE en caso de error
         * **************************************************************************************************************************/

        public void InsertCliente(Cliente c)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + nombreBD))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = "INSERT INTO clientes VALUES (null, @nombre, @documento, @foto, @edad, @genero, @telefono)";

                // Se Configura el tipo de valores
                command.Parameters.Add("@nombre", SqliteType.Text);
                command.Parameters.Add("@documento", SqliteType.Text);
                command.Parameters.Add("@foto", SqliteType.Blob);
                command.Parameters.Add("@edad", SqliteType.Integer);
                command.Parameters.Add("@genero", SqliteType.Text);
                command.Parameters.Add("@telefono", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@nombre"].Value = c.Nombre;
                command.Parameters["@documento"].Value = c.Documento;
                command.Parameters["@foto"].Value = c.Foto;
                command.Parameters["@edad"].Value = c.Edad;
                command.Parameters["@genero"].Value = c.Genero;
                command.Parameters["@telefono"].Value = c.Telefono;

                // Se ejecuta el INSERT
                command.ExecuteNonQuery();
            }
        }

        // Recibe el Objeto Cliente con las propiedades editadas
        // más el documento original , para buscar el registro.
        // Hay que hacer una copia del cliente original y editar
        // los datos necesario antes de llamar este metodo
        public void EditarCliente(Cliente c, string docOriginal)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + nombreBD))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = @"UPDATE clientes SET nombre = @nombre, 
                                                            documento = @documento, 
                                                            foto = @foto,
                                                            edad = @edad,
                                                            genero = @genero,
                                                            telefono = @telefono 
                                                            WHERE documento = @documentoOriginal";

                // Se Configura el tipo de valores
                command.Parameters.Add("@nombre", SqliteType.Text);
                command.Parameters.Add("@documento", SqliteType.Text);
                command.Parameters.Add("@foto", SqliteType.Blob);
                command.Parameters.Add("@edad", SqliteType.Integer);
                command.Parameters.Add("@genero", SqliteType.Text);
                command.Parameters.Add("@telefono", SqliteType.Text);
                command.Parameters.Add("@documentoOriginal", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@nombre"].Value = c.Nombre;
                command.Parameters["@documento"].Value = c.Documento;
                command.Parameters["@foto"].Value = c.Foto;
                command.Parameters["@edad"].Value = c.Edad;
                command.Parameters["@genero"].Value = c.Genero;
                command.Parameters["@telefono"].Value = c.Telefono;
                command.Parameters["@documentoOriginal"].Value = docOriginal;

                // Se ejecuta el UPDATE
                command.ExecuteNonQuery();
            }
        }

        // Comprobar antes de llamar el metodo, que el Cliente no tenga estacionamientos activos
        public void EliminarCliente(Cliente c)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + nombreBD))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = @"DELETE FROM clientes 
                                        WHERE documento = @documento";

                // Se Configura el tipo de valores
                command.Parameters.Add("@documento", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@documento"].Value = c.Documento;

                // Se ejecuta el DELETE
                command.ExecuteNonQuery();
            }
        }

        // Comprueba si existe el documento en la BBDD
        // y devuelve un numero de coincidencias
        // 0 = False y Mayor que 0 = True
        public int ExisteDocumento(string doc)
        {
            int valorDevuelto = 0;

            using (SqliteConnection conn = new SqliteConnection("Data Source=" + nombreBD))
            {
                conn.Open();

                SqliteCommand command = conn.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = "SELECT COUNT(*) FROM clientes WHERE documento = @documento";

                // Se Configura el tipo de valores
                command.Parameters.Add("@documento", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@documento"].Value = doc;

                // Se ejecuta el SELECT
                valorDevuelto = Convert.ToInt32(command.ExecuteScalar());
            }

            return valorDevuelto;
        }

        // Crea Las tablas de la BBDD 'parking.bd'
        public void CreateTablesIfNotExists()
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=" + nombreBD))
            {
                // Se conecta a la BBDD
                conn.Open();

                // Empieza la Trasaccion
                using (SqliteTransaction transaction = conn.BeginTransaction())
                {
                    SqliteCommand command = conn.CreateCommand();

                    // Tabla Clientes
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS clientes
                                        (id_cliente INTEGER PRIMARY KEY,
                                        nombre     TEXT,
                                        documento  TEXT    NOT NULL,
                                        foto       TEXT,
                                        edad       INTEGER,
                                        genero     TEXT,
                                        telefono   TEXT)";
                    command.ExecuteNonQuery();

                    // Tabla Estacionamientos
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS estacionamientos
                                        (id_estacionamiento INTEGER PRIMARY KEY,
                                        id_vehiculo        INTEGER REFERENCES vehiculos (id_vehiculo),
                                        matricula          TEXT    NOT NULL,
                                        entrada            TEXT    NOT NULL,
                                        salida             TEXT,
                                        importe            REAL,
                                        tipo               TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    // Tabla Marcas
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS marcas
                                        (id_marca INTEGER PRIMARY KEY,
                                        marca    TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    // Tabla Vehiculos
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS vehiculos
                                        (id_vehiculo INTEGER PRIMARY KEY,
                                        id_cliente  INTEGER NOT NULL
                                                            REFERENCES clientes (id_cliente),
                                        matricula   TEXT    NOT NULL,
                                        id_marca    INTEGER REFERENCES marcas (id_marca),
                                        modelo      TEXT,
                                        tipo        TEXT    NOT NULL)";
                    command.ExecuteNonQuery();

                    //Commit
                    transaction.Commit();
                }
            }
        }
    }
}
