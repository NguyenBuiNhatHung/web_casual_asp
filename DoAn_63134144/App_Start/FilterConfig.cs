﻿using System.Web;
using System.Web.Mvc;

namespace DoAn_63134144
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
