using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace QuizServer
{
    public partial class GetQuestions : System.Web.UI.Page
    {
        Singleton sg = Singleton.Instance;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (sg.path == null)
                sg.updateSingleton(Request.PhysicalApplicationPath);
            if (sg.isActive)
            {
                Response.ContentType = "text/xml";
                var xdoc = XDocument.Parse(sg.questions);
                var xQuestions = xdoc.Root.Elements("question").ToList();
                xQuestions.ForEach(x => x.Remove());
                var rnd = new Random();
                while(xQuestions.Any())
                {
                    var elementNumber = rnd.Next(xQuestions.Count());
                    xdoc.Root.Add(xQuestions[elementNumber]);
                    xQuestions.RemoveAt(elementNumber);
                }
                Response.Write(xdoc.ToString());
            }
            else
            {
                Response.ContentEncoding = new UTF8Encoding();
                Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><content>0</content>");
            }
        }
    }
}