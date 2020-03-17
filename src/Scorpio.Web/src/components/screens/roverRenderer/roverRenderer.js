import React, { useRef, useEffect, useState } from "react";
import * as THREE from "three";
// import "./sample.stl";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls";
const STLLoader = require("three-stl-loader")(THREE);

const RoverRenderer = () => {
  const mount = useRef(null);

  useEffect(() => {
    let width = mount.current.clientWidth;
    let height = mount.current.clientHeight;

    const scene = new THREE.Scene();
    const camera = new THREE.PerspectiveCamera(1000000, width / height, 1, 500000);
    const renderer = new THREE.WebGLRenderer({ antialias: true });

    camera.position.z = 500;
    renderer.setClearColor("#ff0");
    renderer.setSize(width, height);

    const renderScene = () => {
      renderer.render(scene, camera);
    };

    new STLLoader().load(
      "./sample.stl",
      geom => {
        const roverMaterial = new THREE.MeshBasicMaterial({ color: 0xff00ff });
        const rover = new THREE.Mesh(geom, roverMaterial);
        scene.add(rover);
      },
      progress => console.log(progress),
      error => console.warn(error)
    );

    const handleResize = () => {
      width = mount.current.clientWidth;
      height = mount.current.clientHeight;
      renderer.setSize(width, height);
      camera.aspect = width / height;
      camera.updateProjectionMatrix();
      renderScene();
    };

    mount.current.appendChild(renderer.domElement);
    window.addEventListener("resize", handleResize);

    const animate = () => {
      // cube.rotation.x += 0.01;
      // cube.rotation.y += 0.01;

      renderScene();
      window.requestAnimationFrame(animate);
    };
    animate();

    new OrbitControls(camera, renderer.domElement);

    scene.add(new THREE.AxesHelper(500));
    scene.add(new THREE.GridHelper(2000));

    return () => {
      window.removeEventListener("resize", handleResize);
      mount.current.removeChild(renderer.domElement);

      //scene.remove(cube);
      // geometry.dispose();
      // material.dispose();
    };
  }, []);

  return <div style={{ width: "800px", height: "800px" }} ref={mount} />;
};

export default RoverRenderer;
