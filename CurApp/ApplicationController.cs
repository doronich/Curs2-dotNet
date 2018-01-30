
using System.Windows.Forms;

using System.Configuration;

namespace CurApp
{
    class ApplicationController
    {
        ApplicationContext applicationContext;
        IPresenter presenter;
        readonly string connectionString = FormAuthorization.connectionString;//ConfigurationManager.ConnectionStrings["CurApp.Properties.Settings.dbCuratorConnectionString"].ConnectionString;
        public ApplicationController(ApplicationContext controller)
        {
            applicationContext = controller;
            Model.BL.Model model = new Model.BL.Model(connectionString);
            presenter = new Presenter(new FormAuthorization(applicationContext), model);
        }

        public void Run()
        {
            presenter.Run();
        }
    }
}
