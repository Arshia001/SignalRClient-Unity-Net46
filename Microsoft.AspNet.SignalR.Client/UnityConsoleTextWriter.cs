using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

class UnityConsoleTextWriter : TextWriter
{
    public override Encoding Encoding
    {
        get
        {
            return Encoding.UTF8;
        }
    }

    public override void WriteLine(string value)
    {
        Debug.Log(value);
    }
}
