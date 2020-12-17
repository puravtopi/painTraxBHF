using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// Summary description for SessionManager
/// </summary>
public static class SessionManager
{


    private static HttpSessionState session { get { return HttpContext.Current.Session; } }
    public static bool forwardCC
    {
        get { return Convert.ToBoolean(session["forwardCC"]); }
        set { session["forwardCC"] = value; }
    }

    public static bool forwardPE
    {
        get { return Convert.ToBoolean(session["forwardPE"]); }
        set { session["forwardPE"] = value; }
    }

    public static bool forwardROM
    {
        get { return Convert.ToBoolean(session["forwardPE"]); }
        set { session["forwardPE"] = value; }
    }


}