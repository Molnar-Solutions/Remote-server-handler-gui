import {Grid, InputAdornment, Stack, TextField} from '@mui/material';

import Logo from '../../assets/computer-156951_640.png';
import ActionButton from '../../components/button/button.component';
import Typography from '@mui/material/Typography';
import {useDispatch} from 'react-redux';
import {useRef, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import sendApiRequest, {NetworkMethods} from '../../lib/Network';
import {setUser} from "../../store/reducers/user/user.reducer";
import IconButton from "@mui/material/IconButton";

import VisibilityEnable from "@mui/icons-material/RemoveRedEye";
import VisibilityDisable from "@mui/icons-material/VisibilityOff";

import toast from "react-hot-toast";
import {z} from "zod";

export default function SignIn() {
    const recaptcha = useRef(null);

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isSent, setIsSent] = useState(false);
    const [isPasswordVisible, setIsPasswordVisible] = useState(false);

    const onEmailChange = (event) => setEmail(event.target.value);
    const onPasswordChange = (event) => setPassword(event.target.value);

    const blockSubmitButton = () => {
        setIsSent(true);
        setTimeout(() => {
            setIsSent(false);
        }, 6000);
    }

    const onSubmit = async () => {
        if (!(email && password)) {
            toast.error("Whoops! Email or password field empty!");
            blockSubmitButton();
            return;
        }

        try {
            /* validate e-mail */
            const emailSchema = z.string().email();
            const emailAddress = email;
            emailSchema.parse(emailAddress);

            const response = await sendApiRequest({
                method: NetworkMethods.POST,
                endpoint: 'user/sign-in',
                headers: {
                    "x-public-key": process.env.REACT_APP_PUBLIC_REQUEST_KEY,
                },
                payload: {
                    email: email,
                    password: password,
                },
            });

            if (!response.isSuccess) {
                toast.error('Whoops! Something went wrong. Please try again!');
                setEmail('');
                setPassword('');
                return;
            }

            let apiResponse = response.data ?? response.Data;

            // User signed in.
            dispatch(setUser(apiResponse.data ?? apiResponse.Data));

            // Navigate user to home page.
            navigate('/');
        } catch (e) {
            toast.error('Whoops! Something went wrong. Please try again!');
            blockSubmitButton();
        }
    };

    return (
        <Stack
            flexDirection={"column"}
            justifyContent={"center"}
            alignItems={"center"}
            className={"auth-layout"}
        >
            <Stack
                flexDirection={"column"}
                justifyContent={"center"}
                alignItems={"center"}
                className={"auth-inner-box"}
            >
                <img src={Logo} className="auth-image" alt={'logo'}/>
                <Typography variant="h3" textAlign="center" className={"auth-title"}>
                    Server Handler
                </Typography>
                <br/>
                <br/>

                <TextField
                    title={"E-mail *"}
                    placeholder={"E-mail address"}
                    value={email}
                    fullWidth
                    sx={{ maxWidth: "300px" }}
                    onChange={onEmailChange}
                />
                <br />
                <TextField
                    title={"Password *"}
                    placeholder={"Password"}
                    value={password}
                    type={isPasswordVisible ? "text" : "password"}
                    fullWidth
                    sx={{ maxWidth: "300px" }}
                    onChange={onPasswordChange}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position={"end"}>
                                <IconButton
                                    onClick={() => setIsPasswordVisible(!isPasswordVisible)}
                                >
                                    {isPasswordVisible ? (
                                        <VisibilityEnable/>
                                    ) : (
                                        <VisibilityDisable/>
                                    )}
                                </IconButton>
                            </InputAdornment>
                        ),
                    }}
                />
                <br/>
                { /**/ }
                <br/>
                <ActionButton
                    title={'Sign In'}
                    type="BLUE"
                    action={onSubmit}
                    disabled={isSent}
                />
                <br/>
            </Stack>
        </Stack>
    );
}