﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Common.Logic.Model;
using Vueling.DataAcces.Dao;

namespace Vueling.Business.Logic
{

    public interface ITypeFactory
    {
        IAlumnoFormatoDao TypeTxt();
        IAlumnoFormatoDao TypeJson();
        IAlumnoFormatoDao TypeXml();
    }

}
