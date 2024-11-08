import {useDispatch, useSelector} from 'react-redux';
import {useEffect, useRef, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import sendApiRequest, {NetworkMethods} from '../../lib/Network';
import {getCurrentUser, setUser} from "../../store/reducers/user/user.reducer";

import toast from "react-hot-toast";

export default function SignOut() {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const currentUser = useSelector(getCurrentUser);

    useEffect(() => {
        const sendRequest = async () => {
            const response = await sendApiRequest({
                headers: {
                    'Authorization': 'Bearer ' + currentUser.token
                },
                method: NetworkMethods.POST,
                endpoint: `user/sign-out`,
                payload: {
                    token: currentUser.token,
                },
            })

            if (!response.isSuccess) {
                navigate("/bejelentkezes");
                return;
            }

            navigate("/bejelentkezes");

            toast.success("Successful sign out!");
            dispatch(setUser(null));
        }

        sendRequest();

        return () => {}
    }, []);

    return <h6>Successful sign out!</h6>
}