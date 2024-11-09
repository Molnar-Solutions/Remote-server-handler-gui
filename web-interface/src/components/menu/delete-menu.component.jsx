import React from "react";
import {Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle} from "@mui/material";

export default function DeleteMenu({ open, handleClose, handleDelete }) {
    return (
        <React.Fragment>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="delete-alert-box"
                aria-describedby="delete-alert-box"
            >
                <DialogTitle id="delete-alert-box-title">
                    Delete item
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="delete-alert-box-description">
                        Are you sure you want to delete the following item?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button variant={"contained"} color={"error"} onClick={() => {
                        handleDelete()
                        handleClose()
                    }}>
                        Delete
                    </Button>
                </DialogActions>
            </Dialog>
        </React.Fragment>
    );
}