using System.Collections;
using System.Collections.Generic;

public class Factory<T> {

    List<T> datas;

    public Factory()
   {
        datas = new List<T>();
    }
    
    protected virtual void create()
    {
        T t = default(T);
        datas.Add(t);
    }

    protected virtual T get()
    {
        return datas[0];
    }
}
