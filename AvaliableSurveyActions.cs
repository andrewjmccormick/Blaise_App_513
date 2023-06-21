using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise_App {

    public enum AvailableSurveyActions {
        None = 0,
        /// <summary>
        /// Launch the instrument
        /// </summary>
        Start = 1,
        /// <summary>
        /// Install the instrument locally, suitable for ThickClient or Disconnected 
        /// </summary>
        Install = 2,
        /// <summary>
        /// Upload the collected data to the main server
        /// </summary>
        UploadData = 4,
        Remove = 8,
        ShowDetails = 16,
        /// <summary>
        /// Preload cases from the server onto the device
        /// </summary>
        DownloadCases = 32,
        DeleteData = 64,
        BrowseData = 128,
      
    }

}
