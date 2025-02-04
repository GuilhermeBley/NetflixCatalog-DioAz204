import React, { useState, useEffect } from 'react';
import axios from './api/NetflixApi';
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
    const response = await axios.get('api/GetAllMovies');
    setMovies(response.data);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append('file', file);
    formData.append('movieData', JSON.stringify({ name, description, category }));

    await axios.post('api/Movie', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });

    fetchMovies();
  };

  return (
    <div className="container mt-5">
      <h1 className="text-center mb-4">Bley Movies</h1>

      {/* Add Movie Form */}
      <div className="card mb-4">
        <div className="card-body">
          <h5 className="card-title">Add a New Movie</h5>
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <input
                type="text"
                className="form-control"
                placeholder="Name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <input
                type="text"
                className="form-control"
                placeholder="Description"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <input
                type="text"
                className="form-control"
                placeholder="Category"
                value={category}
                onChange={(e) => setCategory(e.target.value)}
                required
              />
            </div>
            <div className="mb-3">
              <input
                type="file"
                className="form-control"
                onChange={(e) => setFile(e.target.files[0])}
                required
              />
            </div>
            <button type="submit" className="btn btn-primary">
              Add Movie
            </button>
          </form>
        </div>
      </div>

      {/* Movie List */}
      <div className="row">
        {movies.map((movie) => (
          <div key={movie.id} className="col-md-4 mb-4">
            <div className="card h-100">
              <img
                src={movie.imageUrl}
                className="card-img-top"
                alt={movie.name}
                style={{ height: '300px', objectFit: 'cover' }}
              />
              <div className="card-body">
                <h5 className="card-title">{movie.name}</h5>
                <p className="card-text">{movie.description}</p>
                <p className="card-text">
                  <small className="text-muted">{movie.category}</small>
                </p>
              </div>
              <div className="card-footer">
                <small className="text-muted">
                  Added on: {new Date(movie.createdAt).toLocaleDateString()}
                </small>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default App;