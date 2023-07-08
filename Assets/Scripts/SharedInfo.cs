using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SharedInfo
{
    public static int Funds = 100;
    public static int InsurancePayoff = 0;

    public static void resetFunds()
    {
        Funds = 0;
    }
}
