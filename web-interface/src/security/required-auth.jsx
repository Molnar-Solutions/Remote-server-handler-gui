import Typography from '@mui/material/Typography';
import {Outlet, useNavigate} from 'react-router-dom';
import {useDispatch, useSelector} from 'react-redux';
import {useEffect} from 'react';
import {getCurrentUser, setUser} from '../store/reducers/user/user.reducer';
import sendApiRequest, {NetworkMethods} from '../lib/Network';

function displayError(message) {
    return <div style={{textAlign: 'center'}}>
        <Typography variant='h4' color='red'>{message}</Typography>
        <a href='/sign-in'><Typography variant='h4' color='blue'>Sign In</Typography></a>
    </div>;
}

export default function RequiredAuth() {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const currentUser = useSelector(getCurrentUser);

    useEffect(() => {
        if (!currentUser?.token) dispatch(setUser(null));

        const userChecker = async () => {
            const response = await sendApiRequest({
                method: NetworkMethods.POST,
                endpoint: 'user/loggedin',
                headers: {
                    'Authorization': 'Bearer ' + currentUser.token
                },
                payload: {
                    token: currentUser.token
                }
            });

            if (!response.isSuccess) {
                navigate('/sign-in');
                dispatch(setUser(null));
                return;
            }

            const respondedUser = response.data;

            if (!respondedUser) {
                navigate('/sign-in');
                dispatch(setUser(null));
                return;
            }

            dispatch(setUser(respondedUser));
        };

        userChecker();

        return () => {
        }
    }, []);

    const navigateToLogin = () => navigate('/sign-in');

    if (!currentUser.isLogged) {
        navigateToLogin();

        /* Basically, this is unnecessary but to keep react rules in
        mind we have to keep this line. */
        return displayError('Whoops! Something went wrong. Please log in again.');
    }

    return <Outlet/>;
}