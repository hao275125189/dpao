using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpao.dp
{
    class Route
    {
        public static UserInfo Login(UserInfo ui)
        { 
           

            if (ui.Site==2)
            {
                ui = sbobet.Login(ui);
            }
            if (ui.Site ==1 )
            {
                ui = hg.Login(ui);
            }
            return ui;    
        }
        public static UserInfo Odds(UserInfo ui)
        {
            
                ;
            if (ui.Site == 2)
            {
                ui = sbobet.Odds(ui);
            }
            if (ui.Site == 1)
            {
                ui = hg.Odds(ui);
            }
            return ui;
        }

        public static UserInfo Ball(UserInfo ui)
        {

            
            if (ui.Site == 2)
            {
                //ui = sbobet.Odds(ui);
            }
            if (ui.Site == 1)
            {
                ui = hg.Ball(ui);
            }
            return ui;
        }


    }
}
