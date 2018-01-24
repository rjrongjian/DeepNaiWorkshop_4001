using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegeditActivity.Bean
{
    public class RespMessage
    {
        private int _code;//1 成功 2 友好提示 3 错误信息
        private String _message;//提示信息
        public RespMessage(int code,String message)
        {
            _code = code;
            _message = message;
        }

        public RespMessage()
        {
            _code = 0;
            _message = "ok";
        }

        public int code { get => _code; set => _code = value; }
        public string message { get => _message; set => _message = value; }
    }
}
