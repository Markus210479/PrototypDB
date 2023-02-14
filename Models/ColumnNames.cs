using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeDB
{
    public enum BauteilViewColumns
    {
        Materialnummer,
        Bauteilbezeichnung,
        Wert,
        Bauform,
        Spannung,
        Leistung,
        Toleranz,
        Lebensdauer,
        Bestand,
        Bedarf,
        Materialstatus,
        Zusatzinfo1,
        Zusatzinfo2
    }

    public enum SQLBauteilDBColumns
    {
        MATERIALNUMMER,
        BAUTEILBEZEICHNUNG,
        WERT,
        BAUFORM,
        SPANNUNG,
        LEISTUNG,
        TOLERANZ,
        LEBENSDAUER,
        BESTAND,
        BEDARF,
        MATERIALSTATUS,
        ZUSATZINFO1,
        ZUSATZINFO2
    }

    public enum SQLReleasesDBColumns
    {
        RELEASENAME,
        RELEASENUMMER,
        LAYOUT,
        RELEASEINFO
    }

    public enum SQLJunc_Rel_BauDBColumns
    {
        RELID,
        BAUID,
        BEDARF
    }
    
}
