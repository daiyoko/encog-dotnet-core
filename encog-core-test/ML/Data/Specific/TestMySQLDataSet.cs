//
// Encog(tm) Core v3.3 - .Net Version
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
using Encog.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Encog.ML.Data.Specific
{
    [TestClass]
    public class TestMySQLDataSet
    {
        [TestMethod]
        public void MySQLDataSet()
        {
            String SQL = "SELECT `in1`, `in2`, `ideal1` FROM `xor` ORDER BY `ID`";
            int INPUT_SIZE = 2;
            int IDEAL_SIZE = 1;
            String CONNECTION_STRING = "server=localhost;uid=root;password=;database=encog;";

            int bits = IntPtr.Size*8;

            if (bits < 64)
            {
                var data = new MySQLMLDataSet(SQL, INPUT_SIZE, IDEAL_SIZE, CONNECTION_STRING);

                XOR.TestXORDataSet(data);
            }
        }
    }
}
