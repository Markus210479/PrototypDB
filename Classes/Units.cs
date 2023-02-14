using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeDB
{
    public enum WiderstandDim
    {
        Ohm,
        mOhm,
        kOhm,
        MOhm    
    }

    public enum InduktivitaetDim
    {
        Henry,
        mHenry,
        µHenry,
        pHenry
    }

    public enum KapazitaetDim
    {
        Farad,
        mFarad,
        µFarad,
        pFarad
    }

    public enum SpannungDim
    {
        Volt,
        mVolt,
        µVolt,
        kVolt
    }

    public enum LeistungDim
    {
        Watt,
        mWatt,
        µWatt,
        kWatt
    }

    public enum ToleranzDim
    {
        Prozent
    }
}
