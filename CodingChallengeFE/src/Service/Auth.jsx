// src/auth.js

import axios from 'axios'; // Ensure axios is imported

// Utility function to decode JWT tokens manually
export const decodeToken = (token) => {
    try {
        const payload = token.split('.')[1];
        const base64 = payload.replace(/-/g, '+').replace(/_/g, '/'); // Handle URL-safe base64
        const decodedPayload = atob(base64);
        const jsonPayload = decodeURIComponent(
            decodedPayload
                .split('')
                .map(function (c) {
                    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
                })
                .join('')
        );
        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error('Failed to decode token:', error);
        return null;
    }
};

// Store token in localStorage
export const storeToken = (token) => {
    localStorage.setItem('authToken', token);
};

// Retrieve token from localStorage
export const getToken = () => {
    return localStorage.getItem('authToken');
};

// Remove token from localStorage
export const removeToken = () => {
    localStorage.removeItem('authToken');
};

// Check if the user is authenticated
export const isAuthenticated = () => {
    const token = getToken();
    if (!token) return false;

    const decoded = decodeToken(token);
    if (!decoded || !decoded.exp) return false;

    if (Date.now() >= decoded.exp * 1000) {
        removeToken();
        return false;
    }
    return true;
};

// Axios instance with JWT token
export const apiClient = axios.create({
    baseURL: 'https://localhost:7208/api', // Replace with your API URL
    headers: {
        'Content-Type': 'application/json',
    },
});

// Axios interceptor to add Authorization header
apiClient.interceptors.request.use(
    (config) => {
        const token = getToken();
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Optional: Axios interceptor to handle 401 errors
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            // Token might be invalid or expired
            removeToken();
            // Optionally, redirect to login page or notify the user
            window.location.href = '/login'; // Example: redirect to login
        }
        return Promise.reject(error);
    }
);
