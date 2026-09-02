using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Pionex.Net.Enums;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    internal partial class PionexRestClientSpotSharedApi : 
        SharedApiBase,
        IPionexRestClientSpotApiShared,
        IPionexRestClientSpotSharedApi
    {
        private readonly PionexRestClientSpotApi _api;

        private const string _topicId = "PionexSpot";
        private const string _exchangeName = "Pionex";
        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(PionexExchange.Metadata, this);

        private readonly HashSet<string> _knownCommodities = [ 
            "BNOX",  // Brent crude oil
            "CPERX", // Copper
            "GSGX",  // Broad commodity index
            "PALLX", // Palladium
            "PAXG",  // Gold
            "PPLTX", // Platinum
            "SLVX",  // Silver
            "UNGX",  // Natural gas
            "USOX",  // Crude oil
            "XAUT_BTC",   // Gold
            "XAUT",  // Gold
            "XAUT"   // Gold
        ];
        private readonly HashSet<string> _knownEquities = [
            "AAOIX", "AAPLX", "AAX", "ADBEX", "ADIX", "ALBX", "AMATX", "AMDX", "AMKRX", "AMZNX",
            "ANETX", "APLDX", "APPX", "ARMX", "ASMLX", "ASTSX", "AVGOX", "AXTIX", "BABAX", "BAX",
            "BBAIX", "BEX", "BITFX", "BLKX", "BLSHX", "BMNRX", "BOTX", "BRKBX", "BXDCX", "CARX",
            "CATX", "CBRSX", "CCJX", "CEGX", "CF", "CIFRX", "CLSKX", "COHRX", "COINX", "COPXX",
            "COPX", "COSTX", "CRCLX", "CRDOX", "CRWDX", "CRWVX", "CSCOX", "CVXX", "DBAX", "DELLX",
            "DIAX", "DJTX", "DRAMX", "DXYZX", "ENPHX", "EQTX", "ETNX", "EUVX", "EWGX", "EWJX",
            "EWTX", "EWUX", "EWYX", "FCXX", "FEZX", "FLNCX", "FLYX", "FNX", "FRVOX", "GEMIX",
            "GEVX", "GEX", "GLWX", "GMEX", "GOOGLX", "GSX", "HIMSX", "HOODX", "IBMX", "IGVX",
            "INTCX", "INTWX", "IONQX", "IRENX", "ISRGX", "ITAX", "IWMX", "KEYSX", "KOPNX", "KSTRX",
            "LACX", "LCLNX", "LITEX", "LLYX", "LMTX", "LNGX", "LRCXX", "LWLGX", "MARAX", "MCDX",
            "METAX", "MOOX", "MOSX", "MPX", "MRVLX", "MSFTX", "MSTRX", "MUUX", "MUX", "MVLLX",
            "NASAX", "NBISX", "NEEX", "NFLXX", "NIOX", "NKEX", "NLRX", "NOCX", "NOKX", "NTRX",
            "NVDAX", "NVOX", "OKLOX", "ONDSX", "ONX", "OPENX", "ORCLX", "OXYX", "PAYPX", "PDDX",
            "PLTRX", "PYPLX", "QCOMX", "QQQX", "RAMX", "RBLXX", "RDWX", "REMXX", "RGTIX", "RIOTX",
            "RKLBX", "RTXX", "SAPX", "SATSX", "SBETX", "SEX", "SHLDX", "SITMX", "SKDDX", "SKHX",
            "SKHY", "SKUUX", "SLBX", "SMCIX", "SMHX", "SMRX", "SMSN", "SNDKX", "SNOWX", "SNPSX",
            "SOFIX", "SOXLX", "SOXSX", "SOXXX", "SPCX", "SPYX", "SQQQX", "SSOX", "STM", "STRCX",
            "STXX", "SWMRX", "TCOMX", "TELX", "TERX", "TQQQX", "TSEMX", "TSLAX", "TSLLX", "TSMX",
            "TTEX", "TXNX", "UAMYX", "UBERX", "UFOX", "UNHX", "URAX", "URNMX", "USARX", "VCXX",
            "VGKX", "VNQX", "VOLTX", "VRTX", "VSHX", "VSTX", "VTIX", "VXXX", "WDCX", "WULFX",
            "XEX", "XLBX", "XLEX", "XLKX", "XMEX", "XOMX", "XOVRX", "XYZ"
            ];

        public PionexRestClientSpotSharedApi(PionexRestClientSpotApi api)
            : base(
                  api.Exchange,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetBalancesOptions,
                GetBookTickerOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetSpotSymbolsOptions,
                GetSpotTickerOptions,
                GetAllSpotTickersOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                GetSpotOrderTradesOptions,
                GetSpotUserTradeHistoryOptions,
                CancelSpotOrderOptions
            );
        }

    }
}
