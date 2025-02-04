import React, { useState } from 'react';

const AddMovieModal = ({ show, handleClose, handleSubmit }) => {
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [category, setCategory] = useState('');
    const [file, setFile] = useState(null);

    const onSubmit = (e) => {
        e.preventDefault();
        handleSubmit({ name, description, category, file });
        handleClose();
    };

    if (!show) {
        return null;
    }

    return (
        <div className="modal" style={{ display: 'block', backgroundColor: 'rgba(0, 0, 0, 0.5)' }}>
            <div className="modal-dialog">
                <div className="modal-content">
                    <div className="modal-header d-flex justify-content-between">
                        <h5 className="modal-title">Add a New Movie</h5>
                        <button type="button" className="close" onClick={handleClose}>
                            <span>&times;</span>
                        </button>
                    </div>
                    <div className="modal-body">
                        <form onSubmit={onSubmit}>
                            <div className="form-group">
                                <label>Name</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="Enter movie name"
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Description</label>
                                <textarea
                                    className="form-control"
                                    rows="3"
                                    placeholder="Enter movie description"
                                    value={description}
                                    onChange={(e) => setDescription(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Category</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="Enter movie category"
                                    value={category}
                                    onChange={(e) => setCategory(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Image</label>
                                <input
                                    type="file"
                                    className="form-control"
                                    onChange={(e) => setFile(e.target.files[0])}
                                    required
                                />
                            </div>

                            <hr />

                            <button type="submit" className="btn btn-primary">
                                Add Movie
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AddMovieModal;