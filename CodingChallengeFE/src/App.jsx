import { useEffect, useState } from 'react';
import './App.css';
import Login from './Login/Login';
import Home from './Home/Home';

import { Routes, Route } from 'react-router-dom';

const App =()=> {
    return (
        <Routes>
            
            <Route path="/" element={<Login />} />
            <Route path="/home" element={<Home />} />
           
           
        </Routes>
    );
   
}

export default App;