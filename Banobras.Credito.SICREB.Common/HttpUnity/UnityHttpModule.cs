using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using System.Web;
using System.Web.UI;

namespace Banobras.Credito.SICREB.Common.HttpUnity
{
    public class UnityHttpModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        public void Dispose() { }

        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            IHttpHandler currentHandler = HttpContext.Current.Handler;
            HttpContext.Current.Application.GetContainer().BuildUp(
                                currentHandler.GetType(), currentHandler);

            // User Controls are ready to be built up after page initialization is complete
            var currentPage = HttpContext.Current.Handler as Page;
            if (currentPage != null)
            {
                currentPage.InitComplete += OnPageInitComplete;
            }
        }

        // Build up each control in the page's control tree
        private void OnPageInitComplete(object sender, EventArgs e)
        {
            var currentPage = (Page)sender;
            IUnityContainer container = HttpContext.Current.Application.GetContainer();
            container.BuildUp(currentPage);            
            HttpContext.Current.ApplicationInstance.PreRequestHandlerExecute -= OnPreRequestHandlerExecute;
        }

        // Get the controls in the page's control tree excluding the page itself
        private IEnumerable<Control> GetControlTree(Control root)
        {
            foreach (Control child in root.Controls)
            {
                yield return child;
                foreach (Control c in GetControlTree(child))
                {
                    yield return c;
                }
            }
        }
    }
}
