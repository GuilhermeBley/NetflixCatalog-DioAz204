import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [movies, setMovies] = useState([]);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [category, setCategory] = useState('');
  const [file, setFile] = useState(null);

  useEffect(() => {
    fetchMovies();
  }, []);

  const fetchMovies = async () => {
    const response = await axios.get('http://localhost:5000/GetAllMovies');
    setMovies(response.data);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append('file', file);
    formData.append('movieData', JSON.stringify({ name, description, category }));

    await axios.post('http://localhost:5000/Movie', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });

    fetchMovies();
  };

  return (
    <div className="App">
      <h1>Bley Movies</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <input
          type="text"
          placeholder="Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
        <input
          type="text"
          placeholder="Category"
          value={category}
          onChange={(e) => setCategory(e.target.value)}
        />
        <input
          type="file"
          onChange={(e) => setFile(e.target.files[0])}
        />
        <button type="submit">Add Movie</button>
      </form>

      <div className="movie-list">
        {movies.map(movie => (
          <div key={movie.id} className="movie-card">
            <img src={movie.fileUrl} alt={movie.name} />
            <h2>{movie.name}</h2>
            <p>{movie.description}</p>
            <p>{movie.category}</p>
          </div>
        ))}
      </div>
    </div>
  );
}

export default App;