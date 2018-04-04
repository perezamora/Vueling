﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.DataAcces.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Business.Logic;
using Vueling.Common.Logic.Model;
using System.IO;
using Vueling.Common.Logic.Util;
using log4net;
using System.Reflection;

namespace Vueling.DataAcces.Dao.Tests
{

    [TestClass()]
    public class AlumnoTxtDaoTests
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        IAlumnoFormatoDao alumnoDao;
        ITypeFactory factory;

        [TestInitialize]
        public void testInit()
        {
            log.Debug("Entrar metodo testInit: ");
            factory = new FileFactory();
            alumnoDao = factory.TypeTxt();
        }


        [DataRow(1, "pere", "zamora", "3333", "10-03-1978")]
        [DataTestMethod]
        public void AddTest(int id, string name, string apellidos, string dni, string fechaNac)
        {
            log.Debug("Entrar metodo AddTest : ");

            var lfechaNac = fechaNac.Split('-');
            var FechaNac = new DateTime(Convert.ToInt32(lfechaNac[2]), Convert.ToInt32(lfechaNac[1]), Convert.ToInt32(lfechaNac[0]));

            Alumno alumno = new Alumno(id, name, apellidos, dni, FechaNac);
            alumno.Guid = System.Guid.NewGuid().ToString();
            alumno.Edad = alumno.CalcularEdat();
            alumno.FechaCr = alumno.GetTimesTamp(DateTime.Now);

            ConfigUtils.SetValorVarEnvironment("txt");

            alumnoDao.Add(alumno);
            Alumno alumnoTest = LeerAlumnoTxt();
            Assert.IsTrue(alumno.Equals(alumnoTest));
            log.Debug("Salir metodo AddTest : ");
        }

        [TestCleanup]
        public void testClean()
        {
            log.Debug("Entrar metodo testClean: ");
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fullPath = path + "\\" + "alumnos.txt";

            File.Delete(fullPath);
            log.Debug("Salir metodo testClean: ");
        }

        private Alumno LeerAlumnoTxt()
        {
            log.Debug("Entrar metodo LeerAlumno: ");
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fullPath = path + "\\" + "alumnos.txt";

            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            using (StreamReader sw = new StreamReader(fs))
            {
                String text = sw.ReadToEnd();
                string[] fields = text.Split(';');
                return new Alumno(int.Parse(fields[0]), fields[1], fields[2], fields[3], Convert.ToDateTime(fields[4]), Convert.ToInt32(fields[5]), fields[6], fields[7]); 
            }
        }

    }
}