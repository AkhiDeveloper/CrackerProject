// Import the functions you need from the SDKs you need
import {initializeApp} from "https://www.gstatic.com/firebasejs/9.14.0/firebase-app.js";
import {getAnalytics} from "https://www.gstatic.com/firebasejs/9.14.0/firebase-analytics.js";
import {getStorage, ref} from "https://www.gstatic.com/firebasejs/9.14.0/firebase-storage.js";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

function getStorageReference(){
  // Your web app's Firebase configuration
  // For Firebase JS SDK v7.20.0 and later, measurementId is optional
  const firebaseConfig = {
      apiKey: "AIzaSyB_fp1PQTXUlZH8_-btXeHwgqJWYtipPNY",
      authDomain: "cracker-project-developer.firebaseapp.com",
      projectId: "cracker-project-developer",
      storageBucket: "cracker-project-developer.appspot.com",
      messagingSenderId: "458375015062",
      appId: "1:458375015062:web:e8496704adaad6886230b7",
      measurementId: "G-H9XEL5RBZP"
  };

  // Initialize Firebase
  const app = initializeApp(firebaseConfig);
  const analytics = getAnalytics(app);

  //Storage
  const storage = getStorage(app);
  const storageRef = ref(storage)

  return storageRef;
}


function getChild(storage_ref){
    
}