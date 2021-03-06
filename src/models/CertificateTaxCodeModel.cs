using System;
using System.Collections.Generic;
using Newtonsoft.Json;

/*
 * AvaTax API Client Library
 *
 * (c) 2004-2017 Avalara, Inc.
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * @author Ted Spence
 * @author Zhenya Frolov
 */

namespace Avalara.AvaTax.RestClient
{
    /// <summary>
    /// Represents a tax code used by the CertCapture process
    /// </summary>
    public class CertificateTaxCodeModel
    {
        /// <summary>
        /// ID number of the tax code
        /// </summary>
        public Int32? id { get; set; }

        /// <summary>
        /// Name of the tax code
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// Tag of the tax code
        /// </summary>
        public String tag { get; set; }


        /// <summary>
        /// Convert this object to a JSON string of itself
        /// </summary>
        /// <returns>A JSON string of this object</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }
    }
}
