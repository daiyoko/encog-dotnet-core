//
// Encog(tm) Core v3.2 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2014 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Encog.Util;
using Encog.Util.CSV;
using Encog.Util.HTTP;

namespace Encog.ML.Data.Market.Loader
{
    /// <summary>
    /// This class loads financial data from Oanda.
    /// </summary>
    /// TODO: needs to parse <PRE> tag from resulted data (not clean CSV)
    public class DukascopyLoader : IMarketLoader
    {
        #region IMarketLoader Members

        /// <summary>
        /// This is a Dictionary<string, int> of currency pairs and the IDs
        /// used in building a Dukascopy query.
        /// </summary>
        /// <example>int currencyPair = DukascopyData["EURUSD"];</example>
        private static Dictionary<string, int> DukascopyData = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DukascopyLoader"/> class.
        /// </summary>
        public DukascopyLoader()
        {
            if (DukascopyData.Count < 5)
            {
                DukascopyData.Add("AUDJPY", 60);
                DukascopyData.Add("AUDUSD", 10);
                DukascopyData.Add("CADJPY", 767);
                DukascopyData.Add("CHFJPY", 521);
                DukascopyData.Add("EURCHF", 511);
                DukascopyData.Add("EURGBP", 510);
                DukascopyData.Add("EURJPY", 509);
                DukascopyData.Add("EURUSD", 1);
                DukascopyData.Add("GBPCHF", 518);
                DukascopyData.Add("GBPEUR", 516);
                DukascopyData.Add("GBPJPY", 517);
                DukascopyData.Add("GBPUSD", 2);
                DukascopyData.Add("JPYCHF", 515);
                DukascopyData.Add("NZDUSD", 11);
                // TODO: UNSURE OF THESE TWO COMMODITY CROSSES YET - CONFIRM
                DukascopyData.Add("XPDUSD", 336);
                DukascopyData.Add("XPTUSD", 335);
                DukascopyData.Add("USDCAD", 9);
                DukascopyData.Add("USDCHF", 3);
                DukascopyData.Add("USDJPY", 4);
                DukascopyData.Add("XAGUSD", 334);
                DukascopyData.Add("XAUUSD", 333);
            }
        }

        /// <summary>
        /// Load the specified financial data. 
        /// </summary>
        /// <param name="ticker">The ticker symbol to load.</param>
        /// <param name="dataNeeded">The financial data needed.</param>
        /// <param name="from">The beginning date to load data from.</param>
        /// <param name="to">The ending date to load data to.</param>
        /// <returns>A collection of LoadedMarketData objects that represent the data
        /// loaded.</returns>
        public ICollection<LoadedMarketData> Load(TickerSymbol ticker,
                                                  IList<MarketDataType> dataNeeded, DateTime from,
                                                  DateTime to)
        {
            ICollection<LoadedMarketData> result =
                new List<LoadedMarketData>();
            Uri url = BuildURL(ticker, from, to);
            WebRequest http = WebRequest.Create(url);
            var response = (HttpWebResponse) http.GetResponse();

            using (Stream istream = response.GetResponseStream())
            {
                var csv = new ReadCSV(istream, true, CSVFormat.DecimalPoint);

                while (csv.Next())
                {
                    DateTime date = csv.GetDate("date");
                    double open = csv.GetDouble("open");
                    double close = csv.GetDouble("close");
                    double high = csv.GetDouble("high");
                    double low = csv.GetDouble("low");
                    double volume = csv.GetDouble("volume");

                    var data = new LoadedMarketData(date, ticker);
                    
                    data.SetData(MarketDataType.Open, open);
                    data.SetData(MarketDataType.Close, close);
                    data.SetData(MarketDataType.High, high);
                    data.SetData(MarketDataType.Low, low);
                    data.SetData(MarketDataType.Volume, volume);
                    result.Add(data);
                }

                csv.Close();
                istream.Close();
            }
            return result;
        }

        #endregion

        /// <summary>
        /// This method builds a URL to load data from Yahoo Finance for a neural
        /// network to train with.
        /// </summary>
        /// <param name="ticker">The ticker symbol to access.</param>
        /// <param name="from">The beginning date.</param>
        /// <param name="to">The ending date.</param>
        /// <returns>The URL to read from</returns>
        private static Uri BuildURL(TickerSymbol ticker, DateTime from, DateTime to)
        {
            int selectedPair = -1;
            Uri url;

            #region Select Currency Pair
            switch (ticker.Symbol)
            {
                case "AUDJPY":
                    selectedPair = DukascopyData["AUDJPY"];
                    break;
                case "AUDUSD":
                    selectedPair = DukascopyData["AUDUSD"];
                    break;
                case "CADJPY":
                    selectedPair = DukascopyData["CADJPY"];
                    break;
                case "CHFJPY":
                    selectedPair = DukascopyData["CHFJPY"];
                    break;
                case "EURCHF":
                    selectedPair = DukascopyData["EURCHF"];
                    break;
                case "EURGBP":
                    selectedPair = DukascopyData["EURGBP"];
                    break;
                case "EURJPY":
                    selectedPair = DukascopyData["EURJPY"];
                    break;
                case "EURUSD":
                    selectedPair = DukascopyData["EURUSD"];
                    break;
                case "GBPEUR":
                    selectedPair = DukascopyData["GBPEUR"];
                    break;
                case "GBPJPY":
                    selectedPair = DukascopyData["GBPJPY"];
                    break;
                case "GBPUSD":
                    selectedPair = DukascopyData["GBPUSD"];
                    break;
                case "JPYCHF":
                    selectedPair = DukascopyData["JPYCHF"];
                    break;
                case "NZDUSD":
                    selectedPair = DukascopyData["NZDUSD"];
                    break;
                case "XPDUSD":
                    selectedPair = DukascopyData["XPDUSD"];
                    break;
                case "XPTUSD":
                    selectedPair = DukascopyData["XPTUSD"];
                    break;
                case "USDCAD":
                    selectedPair = DukascopyData["USDCAD"];
                    break;
                case "USDCHF":
                    selectedPair = DukascopyData["USDCHF"];
                    break;
                case "USDJPY":
                    selectedPair = DukascopyData["USDJPY"];
                    break;
                case "XAGUSD":
                    selectedPair = DukascopyData["XAGUSD"];
                    break;
                case "XAUUSD":
                    selectedPair = DukascopyData["XAUUSD"];
                    break;
                default:
                    break;
            }

            #endregion Select Currency Pair

            if (selectedPair != -1)
            {
                /*
                 * fromD    =   mm.dd.yyyy
                 * np       =   259, 1000, 1500, 2000
                 * interval =   60, 600, 3600, 1D, 7D, 1MO
                 * DF       =   m/d/Y, m.d.Y, d.m.Y, m-d-Y, d-m-Y
                 * endSym   =   win, unix
                 * split    =   tz, coma, tab (; , tab)
                 */

                // construct the URL
                var mstream = new MemoryStream();
                var form = new FormUtility(mstream, null);

                form.Add("Stock", selectedPair.ToString());
                form.Add("fromD", from.ToString("mm.DD.yyyy"));
                form.Add("np", "2000");
                form.Add("interval", "1D");
                form.Add("DF", "m-d-Y"); // date format
                form.Add("endSym", "win");
                form.Add("split", "coma");

                mstream.Close();

                byte[] b = mstream.GetBuffer();

                String str = "http://www.dukascopy.com/freeApplets/exp/exp.php?"
                             + StringUtil.FromBytes(b);
                url = new Uri(str);
            }
            else
            {
                url = new Uri("");
            }

            return url;
        }

        #region IMarketLoader Members


        public string GetFile(string file)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
