using Model.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurApp
{

    public interface IPresenter
    {
        void Run();
    }
    public class Presenter:IPresenter
    {
        private readonly IView _view;
        private readonly IService _service;

        public Presenter(IView view, IService service)
        {
            _view = view;
            _service = service;

        }

        public void Run()
        {
            _view.ShowForm();
        }

        private void UpdateUserInfo()
        {

        }
    }
}
