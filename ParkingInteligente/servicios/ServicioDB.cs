using Microsoft.Data.Sqlite;
using ParkingInteligente.modelo;
using System;
using System.Collections.Generic;

namespace ParkingInteligente.servicios
{
    class ServicioDB
    {
        // Propiedad de Configuración de aplicaciones con el nombre de la BBDD
        static readonly string nombreBD = Properties.Settings.Default.NombreBD;

        protected ServicioDB()
        { }

        static ServicioDB()
        {
            // TODO: Llamar el metodo 'CreateTablesIfNotExists()' en vez del siguiente:
            CreateDemoDB();
        }

        /**************************************************************************************************************************** 
         * TODO: Ver la forma de controlar excepciones de SQLite, para devolver TRUE en caso de exito y FALSE en caso de error
         * 
         * TODO: Eliminar los metodos inutilizados¿?
         * **************************************************************************************************************************/

        /*******************************************************
            METODOS RELACIONADOS CON EL CLIENTE
         *******************************************************/
        public static void InsertClient(Cliente c)
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
        public static void UpdateClient(Cliente c, string docOriginal)
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
        public static void DeleteClient(string doc)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {

                    // Desvincula los Vehiculos del Estacionamiento, para evitar tener que borrarlos
                    // Vehiculo con ID=1 es un vehiculo eliminado de tipo Generico
                    SqliteCommand desvincularEstacionamientos = connection.CreateCommand();
                    desvincularEstacionamientos.CommandText = @"UPDATE estacionamientos
                    SET id_vehiculo = 1, matricula = '*******'
                    WHERE id_vehiculo IN (SELECT id_vehiculo FROM vehiculos 
                        WHERE id_cliente IN (SELECT id_cliente FROM clientes 
                            WHERE documento = @documento))";

                    desvincularEstacionamientos.Parameters.Add("@documento", SqliteType.Text);
                    desvincularEstacionamientos.Parameters["@documento"].Value = doc;

                    // Elimina los vehiculos del Cliente
                    SqliteCommand eliminarVehiculos = connection.CreateCommand();
                    eliminarVehiculos.CommandText = @"DELETE FROM vehiculos 
                    WHERE id_cliente IN (SELECT id_cliente FROM clientes 
                            WHERE documento = @documento)";

                    eliminarVehiculos.Parameters.Add("@documento", SqliteType.Text);
                    eliminarVehiculos.Parameters["@documento"].Value = doc;

                    // Elimina los vehiculos del Cliente
                    SqliteCommand eliminarCliente = connection.CreateCommand();
                    eliminarCliente.CommandText = @"DELETE FROM clientes 
                    WHERE documento = @documento";

                    eliminarCliente.Parameters.Add("@documento", SqliteType.Text);
                    eliminarCliente.Parameters["@documento"].Value = doc;

                    // Se ejecutan los DELETE
                    desvincularEstacionamientos.ExecuteNonQuery();
                    eliminarVehiculos.ExecuteNonQuery();
                    eliminarCliente.ExecuteNonQuery();

                    // Se ejecuta el Commit
                    transaction.Commit();
                }
            }
        }

        // Comprueba si existe el documento en la tabla Clientes
        public static bool IsExistsDocument(string doc)
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
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        // Devuelve la ID del Cliente (0 en caso de que no exista)
        public static int GetIdClient(string documento)
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
        // Menos el cliente Generico 'No Registrado'
        public static List<Cliente> GetListClients()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM clientes WHERE id_cliente > 0";

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

        // Devuelve El cliente segun la ID
        // En caso de que no exista, devolvera el objeto vacio (string "" y int 0)
        public static Cliente GetClientById(int id)
        {
            Cliente cliente = new Cliente();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM clientes WHERE id_cliente = @idCliente";

                // Se Configura el tipo de valores
                command.Parameters.Add("@idCliente", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@idCliente"].Value = id;

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

        // Comprueba si alguno de los vehiculos de un cliente tiene estacionamiento activo (sin finalizar)
        public static bool IsParked(string documentoCliente)
        {
            bool existe = false;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"Select COUNT(*) FROM estacionamientos 
                    WHERE id_vehiculo IN (SELECT id_vehiculo FROM vehiculos 
                        WHERE id_cliente IN (SELECT id_cliente FROM clientes 
                            WHERE documento = @documento)) AND salida = ''";

                // Se Configura el tipo de valores
                command.Parameters.Add("@documento", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@documento"].Value = documentoCliente;

                // Se ejecuta el SELECT
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        /*******************************************************
            METODOS RELACIONADOS CON LA MARCA del vehiculo
         *******************************************************/
        public static void InsertVehicleBrand(string brand)
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
        public static void UpdateVehicleBrand(string oldBrand, string newBrand)
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
        public static void DeleteBrand(string brand)
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
        public static bool IsExistsVehicleBrand(string brand)
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
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        // Devuelve la ID de la marca (0 en caso de que no exista)
        public static int GetIdVehicleBrand(string brand)
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
        public static List<MarcaVehiculo> GetListVehicleBrands()
        {
            List<MarcaVehiculo> lista = new List<MarcaVehiculo>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM marcas WHERE id_marca > 0";

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
        public static void InsertVehicle(Vehiculo v)
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
        public static void UpdateVehicle(Vehiculo v, string matriculaOriginal)
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
                                                            tipo = @tipo
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
        public static void DeleteVehicle(string matricula)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    // Desvincula el Vehiculo del Estacionamiento, para evitar tener que borrarlos
                    // Vehiculo con ID=1 es un vehiculo eliminado de tipo Generico
                    SqliteCommand desvincularEstacionamientos = connection.CreateCommand();
                    desvincularEstacionamientos.CommandText = @"UPDATE estacionamientos
                    SET id_vehiculo = 1, matricula = '*******'
                    WHERE matricula = @matricula";

                    desvincularEstacionamientos.Parameters.Add("@matricula", SqliteType.Text);
                    desvincularEstacionamientos.Parameters["@matricula"].Value = matricula;

                    desvincularEstacionamientos.ExecuteNonQuery();

                    // Elimina El vehiculo
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @"DELETE FROM vehiculos 
                                        WHERE matricula = @matricula";

                    // Se Configura el tipo de valores
                    command.Parameters.Add("@matricula", SqliteType.Text);

                    // Se asignan los valores
                    command.Parameters["@matricula"].Value = matricula;

                    // Se ejecuta el DELETE
                    command.ExecuteNonQuery();

                    // Se ejecuta el Commit
                    transaction.Commit();
                }
            }
        }

        // Comprueba si existe la Matricula en la tabla Vehiculos
        public static bool IsExistsVehicle(string matricula)
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
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        // Comprueba si existe la Matricula en la tabla Vehiculos
        public static int GetVehicleId(string matricula)
        {
            int id = 0;

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
                id = Convert.ToInt32(command.ExecuteScalar());
            }

            return id;
        }

        // Devuelve la lista de Vehiculos
        // Menos los Vehiculos Genericos 'Eliminado' y 'No Registrado'
        public static List<Vehiculo> GetListVehicles()
        {
            List<Vehiculo> lista = new List<Vehiculo>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM vehiculos WHERE id_vehiculo > 1";

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

        public static List<Vehiculo> GetListClientVehicles(int idCliente)
        {
            List<Vehiculo> lista = new List<Vehiculo>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM vehiculos WHERE id_cliente = @id_cliente";

                // Se Configura el tipo de valores
                command.Parameters.Add("@id_cliente", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@id_cliente"].Value = idCliente;

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

        // Devuelve el vehiculo segun la matricula pasada de argumento
        // En caso de que no exista, devolvera el objeto vacio (string "" y int 0)
        public static bool IsVehicleParked(string matricula)
        {
            bool ispArked = false;

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
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    ispArked = true;
                }
            }

            return ispArked;
        }



        /*******************************************************
            METODOS RELACIONADOS CON EL ESTACIONAMIENTO
         *******************************************************/

        // Inserta un estacionamiento
        public static void InsertParkedVehicle(Estacionamiento e)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = "INSERT INTO estacionamientos VALUES (null, @id_vehiculo, @matricula, @entrada, @salida, @importe, @tipo)";

                // Se Configura el tipo de valores
                command.Parameters.Add("@id_vehiculo", SqliteType.Integer);
                command.Parameters.Add("@matricula", SqliteType.Text);
                command.Parameters.Add("@entrada", SqliteType.Text);
                command.Parameters.Add("@salida", SqliteType.Text);
                command.Parameters.Add("@importe", SqliteType.Real);
                command.Parameters.Add("@tipo", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@id_vehiculo"].Value = e.IdVehiculo;
                command.Parameters["@matricula"].Value = e.Matricula;
                command.Parameters["@entrada"].Value = e.Entrada;
                command.Parameters["@salida"].Value = e.Salida;
                command.Parameters["@importe"].Value = e.Importe;
                command.Parameters["@tipo"].Value = e.Tipo;

                // Se ejecuta el INSERT
                command.ExecuteNonQuery();
            }
        }

        // Recibe el objeto de Estacionamiento editado (manteniendo la id_estacionamiento)
        public static void UpdateParkedVehicle(Estacionamiento e)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                // La ID se genera automaticamente (AUTOINCREMENT)
                command.CommandText = @"UPDATE estacionamientos SET id_vehiculo = @id_vehiculo, 
                                                            matricula = @matricula, 
                                                            entrada = @entrada,
                                                            salida = @salida,
                                                            importe = @importe,
                                                            tipo = @tipo
                                                            WHERE id_estacionamiento = @id_estacionamiento";

                // Se Configura el tipo de valores
                command.Parameters.Add("@id_vehiculo", SqliteType.Integer);
                command.Parameters.Add("@matricula", SqliteType.Text);
                command.Parameters.Add("@entrada", SqliteType.Text);
                command.Parameters.Add("@salida", SqliteType.Text);
                command.Parameters.Add("@importe", SqliteType.Real);
                command.Parameters.Add("@tipo", SqliteType.Text);
                command.Parameters.Add("@id_estacionamiento", SqliteType.Integer);

                // Se asignan los valores
                command.Parameters["@id_vehiculo"].Value = e.IdVehiculo;
                command.Parameters["@matricula"].Value = e.Matricula;
                command.Parameters["@entrada"].Value = e.Entrada;
                command.Parameters["@salida"].Value = e.Salida;
                command.Parameters["@importe"].Value = e.Importe;
                command.Parameters["@tipo"].Value = e.Tipo;
                command.Parameters["@id_estacionamiento"].Value = e.IdEstacionamiento;

                // Se ejecuta el UPDATE
                command.ExecuteNonQuery();
            }
        }

        // Elimina un registro de estacionamiento a partir de su ID
        public static void DeleteParkedVehicle(int id_estacionamiento)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM estacionamientos 
                                        WHERE id_estacionamiento = @id_estacionamiento";

                // Se Configura el tipo de valores
                command.Parameters.Add("@id_estacionamiento", SqliteType.Integer);

                // Se asignan los valores
                command.Parameters["@id_estacionamiento"].Value = id_estacionamiento;

                // Se ejecuta el DELETE
                command.ExecuteNonQuery();
            }
        }

        // Comprueba si el vehiculo esta estacionado actualmente (no tiene fecha de salida)
        public static bool IsActiveParkedVehicle(string matricula)
        {
            bool estacionamientoActivo = false;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT COUNT(*) FROM estacionamientos 
                                        WHERE matricula = @matricula
                                        AND salida = ''";

                // Se Configura el tipo de valores
                command.Parameters.Add("@matricula", SqliteType.Text);

                // Se asignan los valores
                command.Parameters["@matricula"].Value = matricula;

                // Se ejecuta el SELECT
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    estacionamientoActivo = true;
                }
            }

            return estacionamientoActivo;
        }

        // Devuelve el listado de Estacionamientos
        // Tanto activos como Finalizados
        public static List<Estacionamiento> GetListAllParkedVehicles()
        {
            List<Estacionamiento> lista = new List<Estacionamiento>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM estacionamientos";

                // Se ejecuta el SELECT
                using (SqliteDataReader lector = command.ExecuteReader())
                {
                    if (lector.HasRows)
                    {
                        while (lector.Read())
                        {
                            Estacionamiento e = new Estacionamiento();
                            e.IdEstacionamiento = Convert.ToInt32(lector["id_estacionamiento"]);
                            e.IdVehiculo = Convert.ToInt32(lector["id_vehiculo"]);
                            e.Matricula = (string)lector["matricula"];
                            e.Entrada = (string)lector["entrada"];
                            e.Salida = (string)lector["salida"];
                            e.Importe = Convert.ToInt32(lector["importe"]);
                            e.Tipo = (string)lector["tipo"];

                            lista.Add(e);
                        }
                    }
                }
            }

            return lista;
        }

        // Devuelve el listado de Estacionamientos Activos (sin finalizar)
        public static List<Estacionamiento> GetListActiveParkedVehicles()
        {
            List<Estacionamiento> lista = new List<Estacionamiento>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM estacionamientos WHERE salida = ''";

                // Se ejecuta el SELECT
                using (SqliteDataReader lector = command.ExecuteReader())
                {
                    if (lector.HasRows)
                    {
                        while (lector.Read())
                        {
                            Estacionamiento e = new Estacionamiento();
                            e.IdEstacionamiento = Convert.ToInt32(lector["id_estacionamiento"]);
                            e.IdVehiculo = Convert.ToInt32(lector["id_vehiculo"]);
                            e.Matricula = (string)lector["matricula"];
                            e.Entrada = (string)lector["entrada"];
                            e.Salida = (string)lector["salida"];
                            e.Importe = Convert.ToInt32(lector["importe"]);
                            e.Tipo = (string)lector["tipo"];

                            lista.Add(e);
                        }
                    }
                }
            }

            return lista;
        }

        // Devuelve el numero de Coches aparcados actualmente
        public static int GetNumberParkedCars()
        {
            int cochesAparcados;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM estacionamientos WHERE salida = '' AND tipo='Coche'";

                // Se ejecuta el SELECT
                cochesAparcados = Convert.ToInt32(command.ExecuteScalar());
            }

            return cochesAparcados;
        }

        // Devuelve el numero de Motos aparcadas actualmente
        public static int GetNumberParkedMotorcycles()
        {
            int motosAparcados;

            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM estacionamientos WHERE salida = '' AND tipo='Moto'";

                // Se ejecuta el SELECT
                motosAparcados = Convert.ToInt32(command.ExecuteScalar());
            }

            return motosAparcados;
        }


        /*******************************************************
            METODO PARA CREAR LA BBDD EN CASO DE QUE NO EXISTA
         *******************************************************/
        private static void CreateTablesIfNotExists()
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


        public static void CreateDemoDB()
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=" + nombreBD))
            {
                // Se conecta a la BBDD
                connection.Open();

                // Empieza la Trasaccion
                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    SqliteCommand command = connection.CreateCommand();

                    command.CommandText = @"DROP TABLE IF EXISTS estacionamientos";
                    command.ExecuteNonQuery();
                    command.CommandText = @"DROP TABLE IF EXISTS vehiculos";
                    command.ExecuteNonQuery();
                    command.CommandText = @"DROP TABLE IF EXISTS clientes";
                    command.ExecuteNonQuery();
                    command.CommandText = @"DROP TABLE IF EXISTS marcas";
                    command.ExecuteNonQuery();

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

                    // Datos Clientes
                    command.CommandText = @"INSERT INTO 'clientes' ('id_cliente','nombre','documento','foto','edad','genero','telefono') 
                        VALUES (0,'No Registrado','0','',0,'',''),
                                (1,'Acevedo Manríquez María Mireya','12358496A','https://proyparkint.blob.core.windows.net/imgparking/avatar06:11:41.jpg',35,'Mujer','956124578'),
                                (2, 'Aguilar Lorantes Irma', '32659815B', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:13:24.jpg', 32, 'Mujer', '965234589'),
                                (3, 'Alcoverde Martínez Roberto Antonio', '95123678C', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:14:28.jpg', 34, 'Hombre', '654128935'),
                                (4, 'Alvarado Mendoza Oscar', '56489520D', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:23:51.jpg', 40, 'Hombre', '784629514'),
                                (5, 'Serina Byrd', '65841523H', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:16:39.jpg', 39, 'Mujer', '645127895'),
                                (6, 'Ursa Mcdowell', '62845951K', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:17:33.jpg', 30, 'Mujer', '964513498'),
                                (7, 'Channing Melton', '98156343O', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:18:30.jpg', 40, 'Mujer', '651284231'),
                                (8, 'Chantale Barrera', '83492154Y', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:19:27.jpg', 20, 'Mujer', '679512354'),
                                (9, 'Jonah Quinn', '65124578T', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:24:40.jpg', 42, 'Hombre', '635951555'),
                                (10, 'John Daniels', '95152648H', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:26:06.jpg', 33, 'Hombre', '632145791'),
                                (11, 'Alexander Lancaster', '01324807G', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:27:45.jpg', 34, 'Hombre', '654123951'),
                                (12, 'Hollee Pratt', '96234584R', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:20:20.jpg', 19, 'Mujer', '653753159'),
                                (13, 'Fernando Alonso', '95123648J', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:28:15.jpg', 28, 'Hombre', '654789512'),
                                (14, 'Juan Antonio Gonzalez Rodriguez', '26154859G', 'https://proyparkint.blob.core.windows.net/imgparking/avatar06:28:44.jpg', 38, 'Hombre', '651234895')";
                    command.ExecuteNonQuery();

                    // Datos Marcas
                    command.CommandText = @"INSERT INTO 'marcas' ('id_marca','marca') VALUES (0,'No Registrado'), (1,'BMW'),
                        (2, 'Citroen'), (3, 'Renault'), (4, 'Mercedez'), (5, 'Peugeot'), (6, 'Audi'), (7, 'Range Rover'),
                        (8, 'Opel'), (9, 'Hyundai'), (10, 'Ford'), (11, 'Fiat'), (12, 'Jeep'), (13, 'Lexus'), (14, 'MINI'),
                        (15, 'SEAT'), (16, 'Subaru'), (17, 'Mitsubishi'), (18, 'Nissan'), (19, 'Skoda'), (20, 'Porsche'),
                        (21, 'Smart'), (22, 'Tesla'), (23, 'Toyota'), (24, 'Volvo'), (25, 'Volkswagen'), (26, 'SsangYong'),
                        (27, 'MG'), (28, 'Mazda'), (29, 'Land Rover'), (30, 'Jaguar'),(31, 'Dacia'), (32, 'Ferrari'), (33, 'Bentley'),
                        (34, 'Aston Martin'), (35, 'Bugatti'), (36, 'Yamaha'), (37, 'Suzuki'), (38, 'Honda'), (39, 'Aprilia'), (40, 'KTM')";
                    command.ExecuteNonQuery();

                    // Datos Vehiculos
                    command.CommandText = @"INSERT INTO 'vehiculos' ('id_vehiculo','id_cliente','matricula','id_marca','modelo','tipo')
                            VALUES (0,0,'No Registrado',0,'','Generico'),
                                (1,0,'Eliminado',0,'','Generico'),
                                (2, 2, '45778KYB', 2, 'C3', 'Coche'),
                                (3, 11, '4595HHY', 36, 'R1', 'Moto'),
                                (4, 14, '9543YAC', 19, 'Fabia', 'Coche'),
                                (5, 8, '3215KPE', 37, 'Hayabusa', 'Moto'),
                                (6, 6, '9435ODS', 22, 'Model S', 'Coche'),
                                (7,1,'2648KHY',1,'Serie 3','Coche'),
                                (8,1,'5564KIK',1,'Serie 5','Coche'),
                                (9, 8, '8899KIO', 1, 'Serie 1', 'Coche'),
                                (10,3,'8967KKY',37,'GSX','Moto')";
                    command.ExecuteNonQuery();

                    // Datos Estacionamientos
                    command.CommandText = @"INSERT INTO 'estacionamientos' ('id_estacionamiento','id_vehiculo','matricula','entrada','salida','importe','tipo') 
                            VALUES (1,0,'0295GTS','02/02/2022 1:00:33','',0.0,'Coche'),
                                (2, 0, '9315KNM', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (3, 0, '7854HAL', '02/02/2022 1:00:33', '', 0.0, 'Moto'),
                                (4, 1, '*******', '02/02/2022 1:00:33', '07/02/2022 17:20:09', 252.63, 'Coche'),
                                (5, 0, '3694UDM', '02/02/2022 1:00:33', '', 0.0, 'Moto'),
                                (6, 0, '7812MNB', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (7, 4, '9543YAC', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (8, 0, '9764ASD', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (9, 0, '3278KLJ', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (10, 5, '3215KPE', '02/02/2022 1:00:33', '', 0.0, 'Moto'),
                                (11, 0, '8528FTJ', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (12, 0, '6542DTJ', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (13, 7, '2648KHY', '02/02/2022 1:00:33', '', 0.0, 'Coche'),
                                (14, 0, '9644JJH', '02/02/2022 1:00:33', '', 0.0, 'Coche')";
                    command.ExecuteNonQuery();

                    //Commit
                    transaction.Commit();
                }

            }
        }
    }
}
