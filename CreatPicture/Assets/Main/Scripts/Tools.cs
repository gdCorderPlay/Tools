using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools {

    /// <summary>
    /// 获取 整数的长度
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static int GetLength(int num)
    {
        int length = 0;
        while (num > 0)
        {
            length++;
            num /= 10;
        }
        return length;
    }
    public static int GetLength(long num)
    {
        int length = 0;
        while (num > 0)
        {
            length++;
            num /= 10;
        }
        return length;
    }
    /// <summary>
    /// 取整数的第一个10的倍数
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static int Floor(int num)
    {
        int length = GetLength(num);
       num = (int)System.Math.Pow(10, length - 1);
        return num;
    }
    public static long Floor(long num)
    {
        int length = GetLength(num);
        num = (int)System.Math.Pow(10, length - 1);
        return num;
    }
}
