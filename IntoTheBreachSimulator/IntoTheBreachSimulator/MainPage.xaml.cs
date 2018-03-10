using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IntoTheBreachSimulator
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            Content = new PlayArea(this, 8);
		}

        protected override bool OnBackButtonPressed()
        {
            if(Content is PlayArea playArea)
            {
                playArea.UndoLastAction();
            }
            return true;
        }
    }
}
