var socket = io() || {};
socket.isReady = false;
var cur_user_id = null;

window.addEventListener("visibilitychange", function (event) {
  if (document.hidden) {
    console.log("not visible.......");
    socket.emit("CHANGE_TAB");
    if (unityInstance != null) {
      window.unityInstance.SendMessage("NetworkManager", "OnPlayerTabChange");
    }
    // alert("You are not allowed to leave page.");
  } else {
    socket.emit("RETURN_TO_TAB");
    console.log("is visible////////");
    window.focus(function (event) {
      //do something
    });
  }
});

function ConnectVideoChat(data)
{
 
}
function refreshpage()
{
  // console.log("Refresh Page");
  location.reload()
}

// Check for initial media devices
navigator.mediaDevices.enumerateDevices().then((devices) => {
  console.log(" enumerateDevices :::");
  processDevices(devices);
});
navigator.mediaDevices.ondevicechange = function (event) {
  console.log(" ondevicechange :::");
  navigator.mediaDevices.enumerateDevices().then((devices) => {
    processDevices(devices);
  });
};
var isCameraConnected = false;
function processDevices(devices) {
  const cameraPresent = devices.some((device) => device.kind === 'videoinput');
  if (cameraPresent && !isCameraConnected) {
    isCameraConnected = true;
    console.log('Camera is connected');
    socket.emit("JOINT_CAMERA")
    // document.getElementById('WebView-eb7fc8ce-feba-41d8-8471-96d9e8dac280').contentWindow.location.reload(true);
  } else if(isCameraConnected && !cameraPresent) {
    isCameraConnected = false;
    socket.emit("REMOVE_CAMERA")
    console.log('Camera is disconnected');
  }
};



window.addEventListener("load", function () {
  var execInUnity = function (method) {
    if (!socket.isReady) return;

    var args = Array.prototype.slice.call(arguments, 1);
    if (unityInstance != null) {
      window.unityInstance.SendMessage(
        "NetworkManager",
        method,
        args.join("::")
      );
    }
  };

  socket.on("res", function (data) {
    if (data.event == "SEND_OBJECT_STATUS") {
      console.log("OnGetResponse:: ", JSON.stringify(data));
    }
    if (unityInstance != null) {
      window.unityInstance.SendMessage(
        "NetworkManager",
        "OnGetResponse",
        JSON.stringify(data)
      );
    }
  });

  socket.on("PONG", function (msg) {

    if (unityInstance != null) {
      window.unityInstance.SendMessage("NetworkManager", "OnPrintPongMsg");
    }
  });
}); //END_WINDOW.ADDEVENTLISTENER

window.onload = (e) => {
  mainFunction(1000);
};

function checkMicPermission() {
  // navigator.mediaDevices
  //   .getUserMedia({ audio: true })
  //   .then((stream) => {
  //     window.localStream = stream; // A
  //     window.localAudio.srcObject = stream; // B
  //     window.localAudio.autoplay = true; // C
  //   })
  //   .catch((err) => {
  //     // console.error(`you got an error: ${err}`);
  //   });

  // isPermissionAllowed("camera");
  // isPermissionAllowed("microphone");
}

function checkCameraPermission() {
  // navigator.mediaDevices
  //   .getUserMedia({ video: true })
  //   .then((stream) => {
  //     window.localStream = stream; // A
  //     window.localAudio.srcObject = stream; // B
  //     window.localAudio.autoplay = true; // C
  //   })
  //   .catch((err) => {
  //     // console.error(`you got an error: ${err}`);
  //   });

  // isPermissionAllowed("camera");
  // isPermissionAllowed("microphone");
}

function isPermissionAllowed(permissionName) {
  // navigator.permissions
  //   .query({
  //     name: permissionName, //'microphone'
  //   })
  //   .then(function (permissionStatus) {
  //     var data = {
  //       device: permissionName,
  //       status: permissionStatus.state,
  //     };
  //     if (unityInstance != null) {
  //       window.unityInstance.SendMessage(
  //         "NetworkManager",
  //         "OnCheckPermission",
  //         JSON.stringify(data)
  //       );
  //     }

  //     console.log("data+", data);

  //     permissionStatus.onchange = () => {
  //       console.log(
  //         "changed ",
  //         permissionName,
  //         " status :",
  //         permissionStatus.state
  //       );
  //       var data = {
  //         device: permissionName,
  //         status: permissionStatus.state,
  //       };
  //       if (unityInstance != null) {
  //         window.unityInstance.SendMessage(
  //           "NetworkManager",
  //           "OnCheckPermission",
  //           JSON.stringify(data)
  //         );
  //       }
  //     };

  //     // return permissionStatus.state !== 'denied';
  //   })
  //   .catch((error) => {
  //     console.log("Got error :", error);
  //     // return permissionStatus.state === 'denied';
  //   });
}

 function mainFunction(time) {
//   // navigator.mediaDevices.getUserMedia({ audio: true }).then((stream) => {
//   //   var madiaRecorder = new MediaRecorder(stream);
//   //   madiaRecorder.start();

//   //   var audioChunks = [];

//   //   madiaRecorder.addEventListener("dataavailable", function (event) {
//   //     audioChunks.push(event.data);
//   //   });
   
//   //   madiaRecorder.addEventListener("stop", function () {
//       // var audioBlob = new Blob(audioChunks);
//       // audioChunks = [];
//       // var fileReader = new FileReader();

//       // fileReader.readAsDataURL(audioBlob);
//       // fileReader.onloadend = function () {
//       //   // console.log("fileReader.result BEFORE", fileReader.result);
//       //   var base64String = fileReader.result;
//       //   // var converted = base64String.replace('-', '+');
//       //   // converted = converted.replace('_', '/');
//       //   socket.emit("VOICE", base64String);

//       // };
    
//       // madiaRecorder.start();
//       // setTimeout(function () {
//       //   madiaRecorder.stop();
//       // }, time);
//     // });

//     // socket.on("START_SPEAK", function (data) {
//     // madiaRecorder.addEventListener("start", function () {
//     // 	socket.emit("VOICE_START", cur_user_id);
//     // 	console.log("VOICE_START Event.....", madiaRecorder.state);
//     // });
//     // });

//     // setTimeout(function () {
//     //   madiaRecorder.stop();
//     // }, time);
//   });

//   socket.on("UPDATE_VOICE", function (data) {
//     var audio = new Audio(data);
//     audio.play();
//   });

//   socket.on("SPEACK_VOICE", function (userId) {
//     console.log("userId :: ", userId);
//     if (unityInstance != null) {
//       window.unityInstance.SendMessage(
//         "NetworkManager",
//         "OnPlayerVoiceStart",
//         userId
//       );
//     }
//   });
}