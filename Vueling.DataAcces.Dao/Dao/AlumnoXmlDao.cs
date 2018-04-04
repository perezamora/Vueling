﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Common.Logic.Model;
using Vueling.Common.Logic.Util;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using log4net;

namespace Vueling.DataAcces.Dao
{


    public class AlumnoXmlDao : IAlumnoFormatoDao
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Alumno Add(Alumno alumno)
        {
            log.Debug("Entrar metodo Add: " + alumno.ToString());

            var path = FileUtils.getPath();
            try
            {
                if (File.Exists(path))
                {
                    List<Alumno> alumnos;
                    XmlSerializer xSeriz = new XmlSerializer(typeof(List<Alumno>));
                    using (StreamReader r = new StreamReader(path))
                    {
                        String xml = r.ReadToEnd();
                        StringReader stringReader = new StringReader(xml);
                        alumnos = (List<Alumno>)xSeriz.Deserialize(stringReader);
                        alumnos.Add(alumno);
                    }

                    using (FileStream fs1 = new FileStream(path, FileMode.Open))
                        xSeriz.Serialize(fs1, alumnos);
                }
                else
                {
                    List<Alumno> alumnos = new List<Alumno>();
                    XmlSerializer xSeriz = new XmlSerializer(typeof(List<Alumno>));
                    using (FileStream fs1 = new FileStream(path, FileMode.Create))
                    {
                        alumnos.Add(alumno);
                        xSeriz.Serialize(fs1, alumnos);
                    }
                }
                return alumno;
            }
            catch (Exception e)
            {
                log.Debug("Catch Add: " + e);
                throw;
            }

        }

        public List<Alumno> GetAlumnos()
        {
            throw new NotImplementedException();
        }
    }
}
