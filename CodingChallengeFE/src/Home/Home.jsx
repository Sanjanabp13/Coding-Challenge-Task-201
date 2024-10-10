// src/MovieList.js
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { storeToken, apiClient } from '../Service/Auth';
import './MovieList.css'; // Add this for CSS styling
import TaskForm from './TaskForm';

const Home = () => {
    const [tasks, setTask] = useState([]);
    const [tasktoedit,setTasktoedit]=useState('');

    useEffect(() => {
        fetchTask();
    }, []);

    const fetchTask = async () => {
        try {
            const response = await apiClient.get('/Tasks');
            setTask(response.data);
            console.log(response.data);
        } catch (error) {
            console.error('Error fetching Tasks:', error);
        }
    };
    const onDelete=async (id)=>{
        console.log("taskID",id)
        try {
            const response = await apiClient.delete(`/Tasks/${id}`);
            fetchTask();
        } catch (error) {
            console.error('Error deleting Tasks:', error);
        }
    }
    const onEdit=async (id)=>{
        console.log("task")
        
        setTasktoedit(id)
       
    }
    const onAdd=async ()=>{
        console.log("isadded")
        fetchTask();
    }


    return (
        <div>            <h2>Task List</h2>
        <table class="table-auto">
        <thead>
          <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Due date</th>
            <th>Priority</th>
            <th> Status</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
        {tasks.map((task) => (<tr>
            <td>
            {task.title} 
            </td>
            <td>
             {task.description}
            </td>
            <td>
           {task.dueDate}
            </td>
            <td>
           {task.priority}
            </td>
            <td>
            {task.status}
            </td>
            <td>
            <div className="movie-actions">
                            <button onClick={() => onEdit(task)} className="edit-btn">Edit</button>
                            <button onClick={() => onDelete(task.taskID)} className="delete-btn">Delete</button>
                        </div></td> </tr>))}
        
         
         
        </tbody>
      </table>
      <TaskForm onAdd={onAdd} tasktoedit={tasktoedit}/>
      </div>
    );
};

export default Home;
