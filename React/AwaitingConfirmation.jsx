import React from "react";
import migratelyLogo from "../../assets/images/migrately/migrately-logo.png";
import "./userstyles.css";
import { Col, Card, Row, Image } from "react-bootstrap";
import { Link } from "react-router-dom";

const AwaitingConfirmation = () => {
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
                  <h1 className="mb-1 fw-bold">Almost there!</h1>
                  <span>
                    We have sent you an <b>email</b> to the email address you
                    registered with!{" "}
                  </span>
                  <span>
                    Please be sure to use that email to{" "}
                    <b>confirm your account.</b>
                  </span>
                </div>
                <hr className="my-4" />
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </div>
    </React.Fragment>
  );
};

export default AwaitingConfirmation;
