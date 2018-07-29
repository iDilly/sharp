# sharp-rotmg
Just an open source project, feel free to contribute to the cause :heart:.  
The aim of the source is for it to become an exact replica of the real game: [RotMG](https://www.realmofthemadgod.com/).

Any issues/errors detected can be reported/discussed inside the issues section of this Git repository.   
The issues section can be found under this [link](https://github.com/iDilly/sharp/issues).

Some additional information about the source is that it is using the [MongoDB](https://www.mongodb.com/) C# driver as its [Storage](https://github.com/iDilly/sharp/tree/master/common) engine, and the [Nancy](https://github.com/NancyFx/Nancy) framework as the back-end for the [App](https://github.com/iDilly/sharp/tree/master/server) engine.

## Getting started
1. To setup the source, you require the MongoDB server service/executable to be running. You can download it [here](https://www.mongodb.com/download-center#community).   
Alternatively, you can copy and paste this link: `https://www.mongodb.com/download-center#community`.

*The installation is fairly straightforward, it automatically sets up MongoDB to be a start-up service, therefore you do not have to worry about ever managing the executable if you choose this option.*

2. After you have done that, you're basically done setting up the server. All that's left to do is to compile the source code, it comes pre-configured as a localhost `127.0.0.1:80/2050` server. You can however configure it to your liking, by looking at the [`server.json`](https://github.com/iDilly/sharp/blob/master/server/server.json) and [`common.json`](https://github.com/iDilly/sharp/blob/master/common/common.json) files which are found in the root folders of the main source projects.

*To compile the project, you can use the Visual Studio IDE, which is available for free download [here](https://visualstudio.microsoft.com/downloads/).*   
Alternatively, you can copy and paste this link: `https://visualstudio.microsoft.com/downloads/`.

Please keep in mind that this source is always a WIP, so some features might not always work, but if you find any features that are broken or throw errors, please [file an issue](https://github.com/iDilly/sharp/issues) to this Git repository if you can with logs of what happened and how to replicate the problem (if possible).

3. Simply have fun! 

It is *recommended* to spend some time looking through the comments & summaries that the source provides. It should help you understand what code does what and how the source is structured.

### Version : X27.0.2
It is recommended to use the client provided with the source.