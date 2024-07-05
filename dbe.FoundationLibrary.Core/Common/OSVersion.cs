namespace dbe.FoundationLibrary.Core.Common
{
    /// <summary>
    /// 操作系统
    /// </summary>
    public enum OSVersion : int
    {
        /// <summary>
        /// MajorVersion: 10,MinorVersion: 0,Build: (各个版本有不同的Build号)
        /// </summary>
        WindowsServer2022,
        /// <summary>
        /// MajorVersion: 10,MinorVersion: 0,Build: (各个版本有不同的Build号，从22000开始)
        /// </summary>
        Windows11,
        /// <summary>
        /// MajorVersion: 10,MinorVersion: 0,Build: (各个版本有不同的Build号)
        /// </summary>
        WindowsServer2019,
        /// <summary>
        /// MajorVersion: 10,MinorVersion: 0,Build: (各个版本有不同的Build号，从10240开始)
        /// </summary>
        Windows10,
        /// <summary>
        /// MajorVersion: 10,MinorVersion: 0,Build: (各个版本有不同的Build号)
        /// </summary>
        WindowsServer2016,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 3,Build: 9600
        /// </summary>
        Windows8_1Update1,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 3,Build: 9200
        /// </summary>
        Windows8_1,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 3,Build: 9200
        /// </summary>
        WindowsServer2012R2,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 2,Build: 9200
        /// </summary>
        Windows8,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 2,Build: 9200
        /// </summary>
        WindowsServer2012,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 1,Build: 8400
        /// </summary>
        WindowsHomeServer2011,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 1,Build: 7601
        /// </summary>
        WindowsServer2008R2SP1,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 1,Build: 7600.16385
        /// </summary>
        WindowsServer2008R2,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 1,Build: 7601
        /// </summary>
        Windows7ServicePack1,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 1,Build: 7600
        /// </summary>
        Windows7,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 0,Build: 6001
        /// </summary>
        WindowsServer2008,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 0,Build: 6002
        /// </summary>
        WindowsVistaServicePack2,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 0,Build: 6001
        /// </summary>
        WindowsVistaServicePack1,
        /// <summary>
        /// MajorVersion: 6,MinorVersion: 0,Build: 6000.16386
        /// </summary>
        WindowsVista,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 2,Build: 3790
        /// </summary>
        WindowsHomeServer,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 2,Build: 3790.1218
        /// </summary>
        WindowsServer2003R2,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 2,Build: 3790.1180
        /// </summary>
        WindowsServer2003ServicePack1,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 2,Build: 3790
        /// </summary>
        WindowsServer2003,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 2,Build: 3790
        /// </summary>
        WindowsXP64,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 1,Build: 2600
        /// </summary>
        WindowsXPServicePack3,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 1,Build: 2600.2180
        /// </summary>
        WindowsXPServicePack2,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 1,Build: 2600.1105/1106
        /// </summary>
        WindowsXPServicePack1,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 1,Build: 2600
        /// </summary>
        WindowsXP,
        /// <summary>
        /// MajorVersion: 5,MinorVersion: 0,Build: 2195
        /// </summary>
        Windows2000,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 00,Build: 1381
        /// </summary>
        WindowsNT4_00,
        /// <summary>
        /// MajorVersion: 3,MinorVersion: 51,Build: 1057
        /// </summary>
        WindowsNT3_51,
        /// <summary>
        /// MajorVersion: 3,MinorVersion: 50,Build: 807
        /// </summary>
        WindowsNT3_5,
        /// <summary>
        /// MajorVersion: 3,MinorVersion: 10,Build: 528
        /// </summary>
        WindowsNT3_1,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 90,Build: 3000
        /// </summary>
        WindowsMillenium,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 10,Build: 2222A
        /// </summary>
        Windows98SecondEdition,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 10,Build: 1998
        /// </summary>
        Windows98,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 03,Build: 1214
        /// </summary>
        Windows95OEMServiceRelease2_5C,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 03,Build: 1212/1213/1214
        /// </summary>
        Windows95OEMServiceRelease2_1,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 00,Build: 1111
        /// </summary>
        Windows95OEMServiceRelease2,
        /// <summary>
        /// MajorVersion: 4,MinorVersion: 00,Build: 950
        /// </summary>
        Windows95OEMServiceRelease1,
        /// <summary>
        /// 未知系统
        /// </summary>
        Unkown = 0
    }
}