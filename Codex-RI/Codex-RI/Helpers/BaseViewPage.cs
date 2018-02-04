using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace Codex_RI.Helpers
{
    public abstract class BaseViewPage : WebViewPage
    {
        //public virtual new CustomPrincipal User
        //{
        //    get { return base.User as CustomPrincipal; }
        //}
    }
    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        protected ILog Log = LogManager.GetLogger("DefaultLogger");
        public static int Revision
        {
            get
            {
                var version = typeof(MvcApplication).Assembly.GetName().Version;
                return version.Revision;
            }
        }

        public T Temp<T>(string key)
        {
            var value = TempData[key];
            if (value == null)
            {
                return default(T);
            }
            return (T)value;
        }

        //public List<Alert> Alerts
        //{
        //    get
        //    {
        //        return Temp<List<Alert>>("Alerts");
        //    }
        //}

        //public virtual new CustomPrincipal User
        //{
        //    get
        //    {
        //        return base.User as CustomPrincipal;
        //    }
        //}

        /// <summary>
        /// Returns a shallow-copy, immutable select list from ViewData for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ReadOnlyCollection<SelectListItem> SelectList(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            var obj = (List<SelectListItem>)ViewData[key];
            if (obj == null)
            {
                throw new NullReferenceException();
                //    throw new NullReferenceException("ViewBag.{key} is null or could not be cast to List<SelectListItem>. Check that it is set in {controller}Controller".FormatWith(new { key, controller = ViewContext.RouteData.Values["controller"] }));
                //}
            }
            var list = new List<SelectListItem>();
            foreach (var item in obj)
            {
                list.Add(new SelectListItem { Text = item.Text, Value = item.Value, Selected = item.Selected });
            }
            return list.AsReadOnly();
        }
    }
}