﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.DataAcces.Dao.Dao;
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Common.Logic.Util;
using System.Reflection;
using Vueling.Common.Logic.Model;
using System.Data;

namespace Vueling.DataAcces.Dao.Dao.Tests
{
    [TestClass()]
    public class AlumnoSqlDaoTests
    {
        private ICrudDao<Alumno> alumnoCrudDao;
        private IDatabase database;

        [TestInitialize]
        public void TestInit()
        {
            alumnoCrudDao = new AlumnoSqlDao<Alumno>();
            DBFactory factory = new DBFactory();
            database = factory.DBSqlServer();
        }

        [TestCleanup]
        public void TestClean()
        {

        }

        [DataRow("pere", "zamora", "3333", "01-03-1998", 20)]
        [DataTestMethod]
        public void InsertTest(string name, string apellidos, string dni, string fechaNac, int edad)
        {
            var countPreIsrt = this.CountAlumnosTable();

            var lfechaNac = fechaNac.Split('-');
            var FechaNac = new DateTime(Convert.ToInt32(lfechaNac[2]), Convert.ToInt32(lfechaNac[1]), Convert.ToInt32(lfechaNac[0]));

            Alumno alumno = new Alumno(0, name, apellidos, dni, FechaNac, edad, DateTime.Now.ToString("yyyyMMddHHmmssffff"))
            {
                Guid = System.Guid.NewGuid().ToString()
            };

            Alumno alumnoIsrt = alumnoCrudDao.Insert(alumno);
            var countPostIsrt = this.CountAlumnosTable();

            Assert.IsTrue(countPreIsrt < countPostIsrt);
        }


        [DataRow(2)]
        [DataTestMethod]
        public void SelectByIdTest(int id)
        {
            Alumno alumnoSelect = new Alumno();
            alumnoSelect.Id = id;
            Alumno alumnoRet = alumnoCrudDao.SelectById(alumnoSelect);

            Assert.IsTrue(alumnoSelect.Id == alumnoRet.Id);
        }

        private int CountAlumnosTable()
        {

            try
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    var sqlCommand = "SELECT COUNT(*) FROM ALUMNOS";
                    using (IDbCommand command = database.CreateCommand(sqlCommand, connection))
                    {
                        return (Int32)command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}