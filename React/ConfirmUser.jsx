import React, { useEffect } from "react";
import userService from "../../services/userService";
import migratelyLogo from "../../assets/images/migrately/migrately-logo.png";
import "./userstyles.css";
import { Link, useSearchParams, useNavigate } from "react-router-dom";
import { Col, Row, Card, Image } from "react-bootstrap";
import swal from "sweetalert2";

function ConfirmUser() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  useEffect(() => {
    const userEmail = searchParams.get("email");
    const userToken = searchParams.get("token");

    userService.confirmEmail(userEmail, userToken).then(onConfirmSuccess).catch(onConfirmFailure);
  }, []);

  const onConfirmSuccess = () => {
    swal
      .fire("Hooray!", "Confirmation Successful!", "success", {
        button: "Ok",
      })
      .then(navigate("/login"));
  };

  const onConfirmFailure = () => {
    swal.fire("Error", "Confirmation Failed.", "error");
  };

  return (
    <React.Fragment>
      <div className="user-form-background">
        <Row className="align-items-center justify-content-center g-0 min-vh-100">
          <Col lg={5} md={5} className="py-8 py-xl-0">
            <Card>
              <Card.Body className="p-6">
                <div className="mb-4">
                  <Link to="/">
                    <Image src={migratelyLogo} className="mb-4" alt="" />
                  </Link>
                  <h1 className="mb-1 fw-bold">Confirming Email...</h1>
                  <span>Personalized U.S. Visa Application and Journey is Just One Moment Away.</span>
                </div>
                <hr className="my-4" />
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </div>
    </React.Fragment>
  );
}

export default ConfirmUser;
