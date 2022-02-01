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
        public void DeleteClient(string doc)
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
                command.Parameters["@documento"].Value = doc;

                // Se ejecuta el DELETE
                command.ExecuteNonQuery();
            }
        }

        // Comprueba si existe el documento en la tabla Clientes
        public bool IsExistsDocument(string doc)
        {
            bool existe = false;

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
                if(Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        // Devuelve la ID del Cliente (0 en caso de que no exista)
        public int GetIdClient(string documento)
        {
            int idCliente = 0;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT id_cliente FROM clientes WHERE docomento = @docomento";

                // Se Configura el tipo de valores
                command.Parameters.Add("@docomento", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@docomento"].Value = documento;

                // Se ejecuta el SELECT
                idCliente = Convert.ToInt32(command.ExecuteScalar());
            }

            return idCliente;
        }

        // Devuelve la lista de todos los Clientes guardados
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

        // Devuelve El cliente segun el documento pasado
        public Cliente GetClient(string documento)
        {
            Cliente cliente = new Cliente();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM clientes WHERE documento = @documento";

                // Se Configura el tipo de valores
                command.Parameters.Add("@docomento", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@docomento"].Value = documento;

                // Se ejecuta el SELECT
                using (SqliteDataReader lector = command.ExecuteReader())
                {
                    if (lector.HasRows)
                    {
                        lector.Read();

                        cliente.Id = Convert.ToInt32(lector["id_cliente"]);
                        cliente.Nombre = (string)lector["nombre"];
                        cliente.Documento = (string)lector["documento"];
                        cliente.Foto = (string)lector["foto"];
                        cliente.Edad = Convert.ToInt32(lector["edad"]);
                        cliente.Genero = (string)lector["genero"];
                        cliente.Telefono = (string)lector["telefono"];
                    }
                }
            }

            return cliente;
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
        public void InsertVehicleBrand(string brand)
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

        // Comprueba si existe la Marca en la table Marcas
        public bool IsExistsVehicleBrand(string brand)
        {
            bool existe = false;

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
               if(Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        // Devuelve la ID de la marca (0 en caso de que no exista)
        public int GetIdVehicleBrand(string brand)
        {
            int idMarca = 0;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT id_marca FROM marcas WHERE marca = @brand";

                // Se Configura el tipo de valores
                command.Parameters.Add("@brand", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@brand"].Value = brand;

                // Se ejecuta el SELECT
                idMarca = Convert.ToInt32(command.ExecuteScalar());
            }

            return idMarca;
        }

        // Devuelve el listado de todas las Marcas guardadas
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
        public void InsertVehicle(Vehiculo v)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = "INSERT INTO vehiculos VALUES (null, @id_cliente, @matricula, @id_marca, @modelo, @tipo)";

                // Se Configura el tipo de valores
                command.Parameters.Add("@id_cliente", SqliteType.Integer);
                command.Parameters.Add("@matricula", SqliteType.Text);
                command.Parameters.Add("@id_marca", SqliteType.Integer);
                command.Parameters.Add("@modelo", SqliteType.Text);
                command.Parameters.Add("@tipo", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@id_cliente"].Value = v.IdCliente;
                command.Parameters["@matricula"].Value = v.Matricula;
                command.Parameters["@id_marca"].Value = v.IdMarca;
                command.Parameters["@modelo"].Value = v.Modelo;
                command.Parameters["@tipo"].Value = v.Tipo;

                // Se ejecuta el INSERT
                command.ExecuteNonQuery();
            }
        }

        // Recibe el Objeto Vehiculo con las propiedades editadas
        // más la matricula original , para buscar el registro.
        // Hay que hacer una copia del vehiculo original y editar
        // los datos necesario antes de llamar este metodo
        public void UpdateVehicle(Vehiculo v, string matriculaOriginal)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = @"UPDATE vehiculos SET id_cliente = @id_cliente, 
                                                            matricula = @matricula, 
                                                            id_marca = @id_marca,
                                                            modelo = @modelo,
                                                            tipo = @tipo,
                                                            WHERE matricula = @matriculaOriginal";

                // Se Configura el tipo de valores
                command.Parameters.Add("@id_cliente", SqliteType.Integer);
                command.Parameters.Add("@matricula", SqliteType.Text);
                command.Parameters.Add("@id_marca", SqliteType.Integer);
                command.Parameters.Add("@modelo", SqliteType.Text);
                command.Parameters.Add("@tipo", SqliteType.Text);
                command.Parameters.Add("@matriculaOriginal", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@id_cliente"].Value = v.IdCliente;
                command.Parameters["@matricula"].Value = v.Matricula;
                command.Parameters["@id_marca"].Value = v.IdMarca;
                command.Parameters["@modelo"].Value = v.Modelo;
                command.Parameters["@tipo"].Value = v.Tipo;
                command.Parameters["@matriculaOriginal"].Value = matriculaOriginal;

                // Se ejecuta el UPDATE
                command.ExecuteNonQuery();
            }
        }

        // Comprobar antes de llamar el metodo, que el Vehiculo no tenga estacionamientos activos
        public void DeleteVehicle(string matricula)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM vehiculos 
                                        WHERE matricula = @matricula";

                // Se Configura el tipo de valores
                command.Parameters.Add("@matricula", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@matricula"].Value = matricula;

                // Se ejecuta el DELETE
                command.ExecuteNonQuery();
            }
        }

        // Comprueba si existe la Matricula en la tabla Vehiculos
        public bool IsExistsVehicle(string matricula)
        {
            bool existe = false;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM vehiculos WHERE matricula = @matricula";

                // Se Configura el tipo de valores
                command.Parameters.Add("@matricula", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@matricula"].Value = matricula;

                // Se ejecuta el SELECT
                if(Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        // Devuelve la ID de la marca (0 en caso de que no exista)
        public int GetIdVehicle(string matricula)
        {
            int idVehiculo = 0;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT id_vehiculo FROM vehiculos WHERE matricula = @matricula";

                // Se Configura el tipo de valores
                command.Parameters.Add("@matricula", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@matricula"].Value = matricula;

                // Se ejecuta el SELECT
                idVehiculo = Convert.ToInt32(command.ExecuteScalar());
            }

            return idVehiculo;
        }

        public List<Vehiculo> GetListVehicles()
        {
            List<Vehiculo> lista = new List<Vehiculo>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM vehiculos";

                // Se ejecuta el SELECT
                using (SqliteDataReader lector = command.ExecuteReader())
                {
                    if (lector.HasRows)
                    {
                        while (lector.Read())
                        {
                            Vehiculo v = new Vehiculo();
                            v.IdVehiculo = Convert.ToInt32(lector["id_vehiculo"]);
                            v.IdCliente = Convert.ToInt32(lector["id_cliente"]);
                            v.Matricula = (string)lector["matricula"];
                            v.IdMarca = Convert.ToInt32(lector["id_marca"]);
                            v.Modelo = (string)lector["modelo"];
                            v.Tipo = (string)lector["tipo"];

                            lista.Add(v);
                        }
                    }
                }
            }

            return lista;
        }

        /**************************************************************************************************************************** 
         *                                                        TODO
         * **************************************************************************************************************************/

        public Vehiculo GetVehiculo(string matricula)
        {
            //TODO: Buscar un cliente por documento y devolver sus datos
            Vehiculo v = new Vehiculo();

            return v;
        }

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
