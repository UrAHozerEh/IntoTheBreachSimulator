using System;
using System.Collections.Generic;
using System.Text;

namespace IntoTheBreachSimulator
{
    public interface IFullAction
    {
        void Do(ActionStack pActionStack);
        string ToString();
    }
}
