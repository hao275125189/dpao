using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace dpao.dp
{
    class UserInfo
    {
        private string _cUrl;
        public string cUrl
        {
            get { return _cUrl; }
            set { this._cUrl = value; }  
        }
        private CookieContainer _cc;
        public CookieContainer cc
        {
            get { return _cc; }
            set { this._cc = value; }
        }
        private string _User;
        public string User
        {
            get { return _User; }
            set { this._User = value; }
        }
        private string _Pwd;
        public string Pwd
        {
            get { return _Pwd; }
            set { this._Pwd = value; }
        }
        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { this._Status = value; }
        }
        private int _Site;
        public int Site
        {
            get { return _Site; }
            set { this._Site = value; }
        }
        private string _Uid;
        public string Uid
        {
            get { return _Uid; }
            set { this._Uid = value; }
        }
        private string _ODDS;
        public string ODDS
        {
            get { return _ODDS; }
            set { this._ODDS = value; }
        }

        private ArrayList al=new ArrayList();
        public ArrayList Al {
            get { return al; }
            set { this.al = value; }
        }
        private Hashtable ht;
        public Hashtable Ht {
            get { return ht; }
            set { this.ht = value; }
        }
        private string msg = null;
        public string Msg
        {
            get { return msg; }
            set { this.msg = value; }
        }

        private int _iType = 0;
        public int iType
        {
            get { return _iType; }
            set { this._iType = value; }
        }

    }
}
