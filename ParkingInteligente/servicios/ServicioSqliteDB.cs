using Microsoft.Data.Sqlite;
using ParkingInteligente.modelo;
using System;
using System.Collections.Generic;

namespace ParkingInteligente.servicios
{
    class ServicioSqliteDB
    {
        // Propiedad de Configuración de aplicaciones con el nombre de la BBDD
        //      ANTES DE ENTREGAR EL PROYECTO, eliminar de la popieda 'NombreDB' la ruta '..\\..\\bbdd\\'    <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //      La he añadido, para que se guarde la bbdd en GitHub (no guarda la carpeta bin)               <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        readonly string nombreBD = Properties.Settings.Default.NombreBD;

        public ServicioSqliteDB()
        {
            // Crea las tablas si no existen al instanciar el objeto
            CreateTablesIfNotExists();
        }

        /**************************************************************************************************************************** 
         * Ver la forma de controlar excepciones de SQLite, para devolver TRUE en caso de exito y FALSE en caso de error
         * **************************************************************************************************************************/


        /*******************************************************
            METODOS RELACIONADOS CON EL CLIENTE
         *******************************************************/
        public void InsertClient(Cliente c)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

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
        public void UpdateClient(Cliente c, string docOriginal)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

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
        public void DeleteClient(Cliente c)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
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
        // 0 = False o Mayor que 0 = True
        public int IsExistsDocument(string doc)
        {
            int valorDevuelto = 0;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
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

        public List<Cliente> GetListClients()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM clientes";

                // Se ejecuta el SELECT
                using (SqliteDataReader lector = command.ExecuteReader())
                {
                    if (lector.HasRows)
                    {
                        while (lector.Read())
                        {
                            Cliente c = new Cliente();
                            c.Id = Convert.ToInt32(lector["id_cliente"]);
                            c.Nombre = (string)lector["nombre"];
                            c.Documento = (string)lector["documento"];
                            c.Foto = (string)lector["foto"];
                            c.Edad = Convert.ToInt32(lector["edad"]);
                            c.Genero = (string)lector["genero"];
                            c.Telefono = (string)lector["telefono"];

                            lista.Add(c);
                        }
                    }

                }
            }
            return lista;
        }

        /**************************************************************************************************************************** 
         *                                                        TODO
         * **************************************************************************************************************************/
        public bool IsParked(Cliente c)
        {
            //TODO: Comprobar si alguno de los coches del cliente esta actualmente aparcado

            return false;
        }

        /*******************************************************
            METODOS RELACIONADOS CON LA MARCA del vehiculo
         *******************************************************/
        public void InsertVehicleBrand(String brand)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = "INSERT INTO marcas VALUES (null, @marca)";

                // Se Configura el tipo de valores
                command.Parameters.Add("@marca", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@marca"].Value = brand;

                // Se ejecuta el INSERT
                command.ExecuteNonQuery();
            }
        }

        // Recibe el nombre de la marca original y el nombre nuevo para actualizar
        public void UpdateVehicleBrand(string oldBrand, string newBrand)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = @"UPDATE marcas
                                        SET marca = @newBrand 
                                        WHERE marca = @oldBrand";

                // Se Configura el tipo de valores
                command.Parameters.Add("@oldBrand", SqliteType.Text);
                command.Parameters.Add("@newBrand", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@oldBrand"].Value = oldBrand;
                command.Parameters["@newBrand"].Value = newBrand;

                // Se ejecuta el UPDATE
                command.ExecuteNonQuery();
            }
        }

        // Comprobar antes de llamar el metodo, que no hayan coches vinculados con la marca
        public void DeleteBrand(string brand)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM marcas 
                                        WHERE marca = @brand";

                // Se Configura el tipo de valores
                command.Parameters.Add("@brand", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@brand"].Value = brand;

                // Se ejecuta el DELETE
                command.ExecuteNonQuery();
            }
        }

        // Comprueba si existe la marca en la BBDD
        // y devuelve un numero de coincidencias
        // 0 = False o Mayor que 0 = True
        public int IsExistsVehicleBrand(string brand)
        {
            int valorDevuelto = 0;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM marcas WHERE marca = @brand";

                // Se Configura el tipo de valores
                command.Parameters.Add("@brand", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@brand"].Value = brand;

                // Se ejecuta el SELECT
                valorDevuelto = Convert.ToInt32(command.ExecuteScalar());
            }

            return valorDevuelto;
        }

        public List<MarcaVehiculo> GetListVehicleBrands()
        {
            List<MarcaVehiculo> lista = new List<MarcaVehiculo>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM marcas";

                // Se ejecuta el SELECT
                using (SqliteDataReader lector = command.ExecuteReader())
                {
                    if (lector.HasRows)
                    {
                        while (lector.Read())
                        {
                            MarcaVehiculo m = new MarcaVehiculo();
                            m.Id = Convert.ToInt32(lector["id_marca"]);
                            m.Marca = (string)lector["marca"];

                            lista.Add(m);
                        }
                    }

                }
            }
            return lista;
        }


        /*******************************************************
            METODOS RELACIONADOS CON EL VEHICULO
         *******************************************************/




        /*******************************************************
            METODOS RELACIONADOS CON EL ESTACIONAMIENTO
         *******************************************************/



        /*******************************************************
            METODO PARA CREAR LA BBDD EN CASO DE QUE NO EXISTA
         *******************************************************/
        public void CreateTablesIfNotExists()
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                // Se conecta a la BBDD
                connection.Open();

                // Empieza la Trasaccion
                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    SqliteCommand command = connection.CreateCommand();

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
