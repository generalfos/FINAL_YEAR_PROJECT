# Beam me up, ATCA
> 19th October 2022 (By `teamname.final`)

Beam Me Up ATCA is a real-time strategy (RTS) and micro-management style game 
based on the operation of the Australian Telescope Compact Array (also known as 
ATCA or the Paul Wild Observatory). 

## Getting Started

If you want to run the application without building it, you can go [here][1] to 
get the latest build for windows. Alternatively if you want to build the 
application from scratch, or if you don't have a Windows machine do the 
following.

### How to Build

#### Prerequisites

To build this project you will need a couple of things:

- [Unity3D][2] (`Editor 2020.3.38f1`)

Using the Unity Editor Hub, click the drop down button next to `Open` and click
`Add project from disk`. The folder to select is the `BeamMeUpATCA` folder in 
the same directory as this README.

#### Build

After opening the project in the right version of the Unity3D editor, navigate
to `File -> Build Settings`. Click build and a directory to build to. Run the
executable.

## External Data

### API

The project relies on three API endpoints to get some live weather data and 
ATCA position data. This information is gathered from the following:

- [Bureau of Meterology][3]
- [OzForecast][4] 
- [CSIRO: Australia Telescope National Facility][5]

### Unity Plugins

A couple of external plugins were used to make API Development easier:

- `Json.Net` - For JSON Parsing
- `HTMLAgilityPack` - For HTML Parsing and Traversal
  
> These aren't necessary to build the project


[1]: https://jaydenbne.itch.io/beammeupatca?password=DECO3801
[2]: https://unity.com/
[3]: http://www.bom.gov.au/fwo/IDN60801/IDN60801.95734.json
[4]: https://ozforecast.com.au/cgi-bin/weatherstation.cgi?station=11001
[5]: https://www.narrabri.atnf.csiro.au/cgi-bin/Public/atca_live.cgi/
