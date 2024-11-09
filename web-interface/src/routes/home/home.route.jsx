import Logo from "../../assets/computer-156951_640.png";
import {Stack} from "@mui/material";
import Thumbnail from "../../components/thumbnail/thumbnail.component";
import Typography from "@mui/material/Typography";

export default function HomePage()
{
    return <div>
        <Typography variant='h3'>Welcome Administrator!</Typography>
        <br/>
        <br/>
        <Typography variant='h4'>Module selector</Typography>
        <br/>
        <Stack flexDirection={"row"} flexWrap={"wrap"} justifyContent={"flex-start"} alignItems={"center"} gap={2}>
            <Thumbnail title={"File manager"} url={"/filemanager"} bgImage={Logo} />
            <Thumbnail title={"System health"} url={"/systemhealth"} bgImage={Logo} />
        </Stack>
    </div>
}