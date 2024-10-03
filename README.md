# Blended Environment in Mixed Reality Remote Collaboration: User Interaction Experiment

![Concept](https://github.com/Pruetikorn1224/MR-Remote-Collaboration-Blended-Environment/blob/main/Images/Thesis%20images.png)

Recently, mixed reality technology has been integrated into remote collaboration systems, with numerous researchers developing different approaches to enable interactions between local and remote users within a mixed reality platform. These studies have facilitated collaborative scenarios by aligning objects from remote environments with those in the user's local environment to enhance accessibility. This study aims to determine the most effective method of blending environments from two users' rooms across three different conditions.

![Paper](https://drive.google.com/file/d/1_TrrQIYVibXVSC-rlE_0lcZvoH1S-U7C/view?usp=sharing)

## Conditions

![Concept](https://github.com/Pruetikorn1224/MR-Remote-Collaboration-Blended-Environment/blob/main/Images/concept_blender.png)

**(A) Simulated Room 1.**
**(B) Simulated Room 2.**
**(C) Simulated Solid Blended Room.** (Realistic virtual objects)
**(D) Simulated Transparent Blended Room.** (Transparent virtual objects)
**(E) Simulated Partial Blended Room.** (Only specific objects are merged)

## Installation

1. Install [Unity](https://unity.com/download) 2022.3.16 LTS or later.
   
2. Clone the repository
   ```bash
   git clone https://github.com/Pruetikorn1224/MR-Remote-Collaboration-Blended-Environment.git
   ```
   
3. Add the ```folder``` to Unity Hub and open the project in Unity.

4. Navigate to Package Manager, click install the following dependencies
   - ```Meta MR Utility Kit```
   - ```XR Interaction Toolkit```
   - ```TextMeshPro```
   - ```Oculus XR Plugin```
     
5. Add the UPM of the following packages:
   - [Ubiq](https://github.com/UCL-VR/ubiq)
   ```bash
   https://github.com/UCL-VR/ubiq.git#upm
   ```
   - [Depth API](https://github.com/oculus-samples/Unity-DepthAPI)
   ```bash
   com.meta.xr.depthapi
   ```

## Environments

The experiment was conducted in two separate rooms on the same floor of a university building (University College London). If you conduct the experiment in different rooms: 

1. import the room models and replace the models in ```\Prefabs\room1``` and ```\Prefabs\room2```

2. convert the shader to ```Meta/Depth/BiRP/Occlusion Standard```

## Server

This experiment was run on [Ubiq](https://github.com/UCL-VR/ubiq). If you would like to use for testing, change connection definition in ```/Environment/Ubiq Network Scene (Demo)``` to ```Nexus```. Otherwise:

1. Install [Node.js](https://nodejs.org/en) (v20 or later)

2. Download git repository
   ```bash
   git clone https://github.com/UCL-VR/ubiq.git
   ```

3. Navigate to ```Node``` folder
   ```bash
   cd <directory>/ubiq/Node
   ```

4. Run the server
   ```bash
   npm install
   npm start
   ```
5. Change ```Ip``` and ```Port``` in connection definition ```localhost```.

## Data

The results and analysis can be accessed through [Google Drive](https://drive.google.com/file/d/1blmMaflvw7cgCWvbN6pMrY_cjPe9ptNC/view?usp=sharing) and [Microsoft Form](https://forms.office.com/Pages/AnalysisPage.aspx?AnalyzerToken=5cloLe2mdMg0dH7rtEcgcIPKVS1eweEC&id=_oivH5ipW0yTySEKEdmlwgnfwzhE1i5ChttjbPTVOcdUN0tHWU01VkFONTlCMEFRVjlRSVVVRFlaSi4u).

## Contact

**Name:** Pruetikorn Chirattitikarn

[![Outlook][outlook-shield]][outlook-mail]

[![Portfolio][portfolio-shield]][portfolio-url]

[![LinkedIn][linkedin-shield]][linkedin-url]

[![GitHub][github-shield]][github-url]

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[outlook-shield]: https://img.shields.io/badge/Microsoft_Outlook-0078D4?style=for-the-badge&logo=microsoft-outlook&logoColor=white
[outlook-mail]: mailto:p.chirattitikarn@outlook.com
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/ch-pruetikorn/
[portfolio-shield]: https://img.shields.io/badge/Portfolio-255E63?style=for-the-badge&logo=About.me&logoColor=white
[portfolio-url]: https://pruetikorn1224.github.io/portfolio-website/
[github-shield]: https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white
[github-url]: https://github.com/Pruetikorn1224
