// src/MovieForm.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { apiClient } from '../Service/Auth';
//import './MovieForm.css'; // Add this for CSS styling

const TaskForm = ({ tasktoedit, onAdd }) => {
    const [task, setTask] = useState({
        title: '',
        description: '',
        dueDate: '',
        priority: '',
        status: ''
    });

    useEffect(() => {
        if (tasktoedit != '') {
            setTask(tasktoedit);
           // fetchTask();
        }
    }, [tasktoedit]);
    const fetchTask = async () => {
        try {
            const response = await apiClient.get('/Tasks/${tasktoedit}');
            
            setTask(response.data);
            console.log(response.data);
        } catch (error) {
            console.error('Error fetching Tasks:', error);
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setTask({ ...task, [name]: value });
    };


    const handleSubmit = async (e) => {
        e.preventDefault();
        if (task.taskID) {
            await apiClient.put(`/Tasks/${task.taskID}`, task);
        } else {
            await apiClient.post(`/Tasks`, task);
        }
        onAdd();
        setTask({
            title: '',
            description: '',
            dueDate: '',
            priority: '',
            status: ''
        });
    };
    
   

    return (
        <form onSubmit={handleSubmit} className="movie-form">
            <h2>{task.taskID ? 'Edit task' : 'Add task'}</h2>
            <input
                type="text"
                name="title"
                value={task.title}
                onChange={handleChange}
                placeholder="Title"
                required
            />
            <input
                type="text"
                name="description"
                value={task.description}
                onChange={handleChange}
                placeholder="description"
                required
            />
            <input
                type="date"
                name="dueDate"
                value={task.dueDate}
                onChange={handleChange}
                placeholder="DueDate"
                required
            />
            <input
                type="text"
                name="priority"
                value={task.priority}
                onChange={handleChange}
                placeholder="priority"
                required
            />
            <input
                type="text"
                name="status"
                
                value={task.status}
                onChange={handleChange}
                placeholder="status"
                required
            />
            <button type="submit">{task.taskID ? 'Update' : 'Add'}</button>
        </form>
    );
};

export default TaskForm;
